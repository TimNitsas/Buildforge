using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Contribution;

public sealed class ContributionViewModelDesignTime : ContributionViewModel
{
    public ContributionViewModelDesignTime() : base(new MockContributionClient())
    {
        foreach (var contribution in MockContributionClient.GetMockData().Contributions)
        {
            Contributions.Add(new ContributionItemViewModel(contribution));
        }
    }
}