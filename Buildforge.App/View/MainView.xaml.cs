using Buildforge.App.ViewModel;

namespace Buildforge.App;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}