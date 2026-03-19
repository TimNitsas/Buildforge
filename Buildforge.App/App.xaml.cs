using Buildforge.App.ViewModel;
using Buildforge.Client.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Diagnostics;
using Velopack;
using Velopack.Exceptions;
using Velopack.Sources;

namespace Buildforge.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; }

    static App()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<BuildViewModel>();

        services.AddSingleton<IBuildforgeClient, MockBuildforgeClient>();

        Services = services.BuildServiceProvider();
    }

    [STAThread]
    private static void Main(string[] args)
    {
        VelopackApp.Build().Run();

        RegisterProtocol();

        using var cts = new CancellationTokenSource();

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