using Buildforge.App.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Buildforge.App;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}