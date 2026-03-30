namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemContributionViewModel : ObservableObject
{
    [ObservableProperty]
    private string? user;

    public BuildItemContributionViewModel(Client.V1.BuildContribution buildContribution)
    {
        User = buildContribution.User;
    }
}
