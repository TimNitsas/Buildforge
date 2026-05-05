namespace Buildforge.App.ViewModel.Build;

public sealed class BuildItemContributionViewModelDesignTime : BuildItemContributionViewModel
{
    public BuildItemContributionViewModelDesignTime() : base(GetContributions())
    {
    }

    private static IEnumerable<Client.V1.BuildContribution> GetContributions()
    {
        for (int i = 0; i < new Random().Next(1, 10); i++)
        {
            yield return BuildItemContributionItemViewModelDesignTime.GetBuildContribution();
        }
    }
}