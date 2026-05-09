using Buildforge.App.ViewModel.Root;

namespace Buildforge.App.View;

public partial class RootView : Window
{
    public RootView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<RootViewModel>();
    }
}