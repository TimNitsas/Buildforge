using Buildforge.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Velopack;

namespace Buildforge.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; }

    static App()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();

        Services = services.BuildServiceProvider();
    }

    [STAThread]
    private static void Main(string[] args)
    {
        VelopackApp.Build().Run();

        App app = new();

        app.InitializeComponent();

        app.Run();
    }
}