using Buildforge.App.ViewModel.Contribution;

namespace Buildforge.App.View;

public partial class ContributionView : UserControl
{
    public ContributionView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<ContributionViewModel>();
    }
}