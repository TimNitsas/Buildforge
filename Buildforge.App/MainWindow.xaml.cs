using Buildforge.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Buildforge.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}