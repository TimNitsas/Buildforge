namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemContributionItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? user;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private string? description;

    public BuildItemContributionItemViewModel(Client.V1.BuildContribution buildContribution)
    {
        User = buildContribution.User;

        Id = buildContribution.Id;

        Description = buildContribution.Description.Replace("\n", " ");
    }
}
