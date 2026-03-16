using System.Windows;
using Velopack;

namespace Buildforge.App;

public partial class App : Application
{
    [STAThread]
    private static void Main(string[] args)
    {
        VelopackApp.Build().Run();

        App app = new();

        app.InitializeComponent();

        app.Run();
    }
}