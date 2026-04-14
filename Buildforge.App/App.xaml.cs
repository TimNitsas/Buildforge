using Buildforge.App.Cli;
using Buildforge.App.Domain.Build;
using Buildforge.App.Domain.Token;
using Buildforge.App.Event;
using Buildforge.App.Messaging;
using Buildforge.App.ViewModel.Authentication;
using Buildforge.App.ViewModel.Contribution;
using Buildforge.App.ViewModel.Main;
using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.Messaging;
using Polly;
using System.Net.Http;
using Velopack;
using Velopack.Exceptions;
using Velopack.Sources;

namespace Buildforge.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; }

    private static readonly Channel<string> ArgsChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
    {
        SingleWriter = true,
        SingleReader = true,
        AllowSynchronousContinuations = false
    });

    private const string ProtocolKey = "buildforge";

    private static Mutex? SingleInstanceMutex;

    static App()
    {
        bool useMock = true;

        Dispatcher.CurrentDispatcher.ShutdownFinished += (s, e) =>
        {
            SingleInstanceMutex?.ReleaseMutex();
        };

        var services = new ServiceCollection();

        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<BuildViewModel>();

        services.AddSingleton<AuthenticationViewModel>();

        services.AddSingleton<ContributionViewModel>();

        services.AddSingleton<TokenHandler>();

        services.AddSingleton<BuildHandler>();

        services.AddSingleton(new BuildLocator(Environment.CurrentDirectory));

        if (useMock)
        {
            services.AddSingleton<IBuildClient, MockBuildforgeClient>();

            services.AddSingleton<IAuthenticationClient, MockAuthenticationClient>();
        }
        else
        {
            string service = "http://localhost:5157";

            var client = new HttpClient();

            services.AddSingleton<IBuildClient>(sp =>
            {
                return new BuildClient(service, client);
            });

            services.AddSingleton<IAuthenticationClient>(sp =>
            {
                return new AuthenticationClient(service, client);
            });


        Services = services.BuildServiceProvider();
    }

    [STAThread]
    private static void Main(string[] args)
    {
        using var cts = new CancellationTokenSource();

        EnforceSingleInstance(args, cts.Token);

        VelopackApp.Build().Run();

        Task.Run(async () => ReadCommands(cts.Token), cts.Token);

        Task.Run(async () => LongPollUpdates(cts.Token), cts.Token);

        Task.Run(async () => TickTimeLoop(cts.Token), cts.Token);

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

    private static async Task TickTimeLoop(CancellationToken ct)
    {
        IMessenger messenger = Services.GetRequiredService<IMessenger>();

        while (!ct.IsCancellationRequested)
        {
            messenger.Send(TickTimeMessage.Instance);

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }

    private static async Task LongPollUpdates(CancellationToken ct)
    {
        var client = Services.GetRequiredService<IBuildClient>();

        var eventPublisher = Services.GetRequiredService<EventPublisher>();

        static TimeSpan RetryLogic(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Min(60, Math.Pow(2, attempt)));
        }

        while (!ct.IsCancellationRequested)
        {
            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(RetryLogic);

            await retryPolicy.ExecuteAsync(async () =>
            {
                foreach (var item in await client.GetUpdatesAsync(ct))
                {
                    eventPublisher.Publish(item);
                }
            });

            await Task.Delay(TimeSpan.FromSeconds(1), ct);
        }
    }

    private static async Task ReadCommands(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (!await ArgsChannel.Reader.WaitToReadAsync(ct))
            {
                break;
            }

            var args = await ArgsChannel.Reader.ReadAsync(ct);

            var protocolPrefix = $"{ProtocolKey}:";

            if (args.StartsWith(protocolPrefix))
            {
                args = args.Replace(protocolPrefix, string.Empty);
            }

            var result = DotMake.CommandLine.Cli.Parse<RootCli>(args);

            if (result.ParseResult.Errors.Any())
            {
                continue;
            }

            await result.ParseResult.InvokeAsync(cancellationToken: ct);
        }
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

            await ArgsChannel.Writer.WriteAsync(args, ct);
        }
    }

    private static void RegisterProtocol()
    {
        using var key = Registry.CurrentUser.CreateSubKey($"SOFTWARE\\Classes\\{ProtocolKey}");

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