namespace Buildforge.App.View;

public partial class BuildView : UserControl
{
    public BuildView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<BuildViewModel>();
    }
}