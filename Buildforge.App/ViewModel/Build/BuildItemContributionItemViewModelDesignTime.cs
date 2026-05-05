using Bogus;
using Buildforge.Client.V1;
using RandomFriendlyNameGenerator;

namespace Buildforge.App.ViewModel.Build;

public sealed class BuildItemContributionItemViewModelDesignTime : BuildItemContributionItemViewModel
{
    public BuildItemContributionItemViewModelDesignTime() : base(GetBuildContribution())
    {
    }

    public static BuildContribution GetBuildContribution()
    {
        var faker = new Faker();

        return new BuildContribution()
        {
            User = NameGenerator.PersonNames.Get(),
            Id = new Random().Next(1, 1_000_000).ToString(),
            Description = faker.Lorem.Sentences(3),
        };
    }
}