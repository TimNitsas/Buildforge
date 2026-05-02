using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Contribution;

public sealed partial class ContributionItemBuildViewModel : ObservableObject
{
    [ObservableProperty]
    private string id;

    [ObservableProperty]
    private string url;

    [ObservableProperty]
    private string status;

    public ContributionItemBuildViewModel(ContributionBuild build)
    {
        Id = build.Id;

        Url = build.Url;

        Status = build.Status;
    }
}
