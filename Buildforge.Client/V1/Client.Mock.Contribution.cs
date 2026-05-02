using Bogus;
using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockContributionClient : IContributionClient
{
    public Task<ContributionResult> GetContributionsAsync(int? skip = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMockData());
    }

    public static ContributionResult GetMockData()
    {
        var result = new ContributionResult();

        var faker = new Faker();

        var random = new Random(42);

        for (int i = 0; i < 100; i++)
        {
            var files = Enumerable.Range(1, random.Next(1, 20)).Select(i => new ContributionFile()
            {
                Path = "some_path",
                Size = i * 1024
            });

            var builds = Enumerable.Range(1, random.Next(1, 5)).Select(x => new ContributionBuild()
            {
                Id = (3 * i * x).ToString(),
                Status = "Building",
                Url = "https://build.system.url/?build_id=id"
            });

            result.Contributions.Add(new Client.V1.Contribution()
            {
                User = NameGenerator.PersonNames.Get(),
                Id = (i * i + i).ToString(),
                Description = faker.Lorem.Sentences(3),
                CommitDate = DateTime.Now.AddMinutes(-random.Next(1, 60 * 4)),
                Files = [.. files],
                Builds = [.. builds]
            });
        }

        return result;
    }
}