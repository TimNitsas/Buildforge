using Bogus;
using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockBuildforgeClient : IBuildClient
{
    private static readonly Random Random = new Random(42);

    public async Task<BuildResult> GetBuildAsync(int? skip = null, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        return GetBuildResult();
    }

    public static BuildResult GetBuildResult()
    {
        var buildResult = new BuildResult
        {
            Builds = []
        };

        for (int i = 0; i < 100; i++)
        {
            buildResult.Builds.Add(new Build()
            {
                Id = $"custom-build-{Random.Next(100_000, 1_000_000)}",
                Name = NameGenerator.Identifiers.Get(),
                Status = BuildStatus[Random.Next(BuildStatus.Count)](),
                Target = Targets[Random.Next(Targets.Count)],
                Contributions = [.. GetContributions()],
                Platform = Platforms[Random.Next(Platforms.Count)],
                Crashes = [.. GetCrashes()]
            });
        }

        return buildResult;
    }

    private static IEnumerable<BuildCrash> GetCrashes()
    {
        var faker = new Faker();

        for (int i = 0; i < Random.Next(0, 100); i++)
        {
            yield return new BuildCrash()
            {
                User = faker.Name.FullName(),
                Date = DateTime.Now.AddDays(-Random.Next(1, 5)),
                PlayTime = TimeSpan.FromMinutes(Random.Next(1, 60))
            };
        }
    }

    private static IEnumerable<BuildContribution> GetContributions()
    {
        var faker = new Faker();

        for (int i = 0; i < Random.Next(0, 20); i++)
        {
            yield return new BuildContribution()
            {
                User = faker.Name.FullName(),
                Description = faker.Lorem.Sentences(2),
                Id = $"change-{i}"
            };
        }
    }

    private static readonly List<string> Targets = new List<string>()
    {
        "Release",
        "Debug",
    };

    private static readonly List<string> Platforms = new List<string>()
    {
        "Pc",
        "Playstation",
        "Xbox",
    };

    private static readonly List<Func<BuildStatus>> BuildStatus =
    [
        () => new BuildStatusFailed(),
        () => new BuildStatusQueued(),
        () => new BuildStatusActive(),
        () => new BuildStatusSuccess()
        {
            BuildTime = TimeSpan.FromMinutes(Random.Next(1, 240)),
            StartTime = DateTime.UtcNow - TimeSpan.FromSeconds(Random.Next(240, 480)),
            Bytes = 1024 * 1024 * 1024  * (long)Random.Next(20, 30),
        }
    ];

    public Task<ICollection<BuildStatus>> GetUpdatesAsync(CancellationToken cancellationToken)
    {
        if (Random.NextDouble() > 0.5f)
        {
            throw new Exception("Mocked");
        }

        return Task.FromResult<ICollection<BuildStatus>>(Array.Empty<BuildStatus>());
    }

    public Task<FileResponse> DownloadBuildAsync(string? buildId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}