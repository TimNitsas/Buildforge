using Buildforge.App.ViewModel.Main;
using Vanara.PInvoke;

namespace Buildforge.App.View;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}