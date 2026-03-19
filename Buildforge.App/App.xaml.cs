using Buildforge.App.ViewModel;
using Buildforge.Client.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Channels;
using System.Windows.Threading;
using Velopack;
using Velopack.Exceptions;
using Velopack.Sources;

namespace Buildforge.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; }

    private static Mutex? SingleInstanceMutex;

    static App()
    {
        Dispatcher.CurrentDispatcher.ShutdownFinished += (s, e) =>
        {
            SingleInstanceMutex?.ReleaseMutex();
        };

        var services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<BuildViewModel>();

        services.AddSingleton<IBuildforgeClient, MockBuildforgeClient>();

        Services = services.BuildServiceProvider();
    }

    [STAThread]
    private static void Main(string[] args)
    {
        using var cts = new CancellationTokenSource();

        EnforceSingleInstance(args, cts.Token);

        VelopackApp.Build().Run();

        RegisterProtocol();

        var updateAppTask = Task.Run(async () =>
        {
            await UpdateLoop(cts.Token);
        });

        App app = new();

        app.InitializeComponent();

        app.Exit += async (sender, e) =>
        {
            cts.Cancel();

            try
            {
                await updateAppTask;
            }
            catch (OperationCanceledException)
            {
            }
        };

        app.Run();
    }

    private static void EnforceSingleInstance(string[] args, CancellationToken ct)
    {
        var mutex = new Mutex(true, nameof(Buildforge), out bool ownership);

        if (ownership)
        {
            SingleInstanceMutex = mutex;

            Task.Run(async () => await RunPipeServer(ct), ct);
        }
        else
        {
            using var client = new NamedPipeClientStream(nameof(Buildforge));

            client.Connect();

            using StreamWriter writer = new(client);

            writer.Write(string.Join(" ", args));

            writer.Flush();

            Environment.Exit(0);
        }
    }

    private static async Task RunPipeServer(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var options = PipeOptions.Asynchronous;

            using var server = new NamedPipeServerStream(nameof(Buildforge), PipeDirection.InOut, 1, PipeTransmissionMode.Message, options);

            await server.WaitForConnectionAsync(ct);

            using var reader = new StreamReader(server, Encoding.UTF8);

            string args = await reader.ReadToEndAsync(ct);

            string[] split = args.Split(' ');

            await ArgsChannel.Writer.WriteAsync(split, ct);
        }
    }

    private static void RegisterProtocol()
    {
        using var key = Registry.CurrentUser.CreateSubKey($"SOFTWARE\\Classes\\buildforge");

        string applicationLocation = typeof(App).Assembly.Location.Replace("dll", "exe");

        key.SetValue(string.Empty, "URL:Build Forge");

        key.SetValue("URL Protocol", string.Empty);

        using var commandKey = key.CreateSubKey(@"shell\open\command");

        commandKey.SetValue(string.Empty, $"\"{applicationLocation}\" \"%1\"");
    }

    private static async Task UpdateLoop(CancellationToken ct)
    {
        var source = new GithubSource("https://github.com/TimNitsas/Buildforge", null, false);

        var updateManager = new UpdateManager(source);

        while (!ct.IsCancellationRequested)
        {
            UpdateInfo? newVersion;

            try
            {
                newVersion = await updateManager.CheckForUpdatesAsync();
            }
            catch (NotInstalledException) when (!Debugger.IsAttached)
            {
                throw;
            }

            if (newVersion != null)
            {
                await updateManager.DownloadUpdatesAsync(newVersion, cancelToken: ct);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), ct);
        }
    }
}