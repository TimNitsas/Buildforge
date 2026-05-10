using Bogus;
using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockContributionClient : IContributionClient
{
    private static readonly Random Random = new Random(42);

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

            var builds = Enumerable.Range(1, random.Next(4, 10)).Select(x => new ContributionBuild()
            {
                Id = (3 * i * x).ToString(),
                Status = "Building",
                Url = "https://build.system.url/?build_id=id",
                Branch = "Main",
            });

            result.Contributions.Add(new Client.V1.Contribution()
            {
                User = NameGenerator.PersonNames.Get(),
                Id = (i * i + i).ToString(),
                Description = faker.Lorem.Sentences(3),
                CommitDate = DateTime.Now.AddMinutes(-random.Next(1, 60 * 4)),
                Files = [.. files],
                Builds = [.. builds],
                Tags = [.. GetTags()],
                Branches = [.. GetBranches()]
            });
        }

        return result;
    }

    private static IEnumerable<string> GetTags()
    {
        if (Random.NextDouble() < 0.10)
        {
            yield return "Validation Bypass";
        }

        if (Random.NextDouble() < 0.05)
        {
            yield return "Build Fix";
        }
    }

    private static IEnumerable<string> GetBranches()
    {
        yield return "Main";

        if (Random.NextDouble() < 0.05)
        {
            yield return "Live";
        }
    }
}