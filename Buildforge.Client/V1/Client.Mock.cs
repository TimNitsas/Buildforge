using Bogus;
using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockBuildforgeClient : IBuildClient
{
    private static readonly Random Random = new Random(42);

    public async Task<BuildResult> GetBuildsAsync(int? skip = null, CancellationToken cancellationToken = default)
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
        () => new BuildStatusActive()
        {
            StartTime = DateTime.UtcNow - TimeSpan.FromSeconds(Random.Next(240, 480)),
            EstimatedTimeToCompletion = DateTime.UtcNow + TimeSpan.FromSeconds(Random.Next(240, 480))
        },
        () => new BuildStatusSuccess()
        {
            BuildTime = TimeSpan.FromMinutes(Random.Next(1, 240)),
            StartTime = DateTime.UtcNow - TimeSpan.FromSeconds(Random.Next(240, 480)),
            Bytes = 1024 * 1024 * 1024  * (long)Random.Next(20, 30),
        }
    ];

    public Task<ICollection<Build>> SubscribeAsync(CancellationToken cancellationToken)
    {
        if (Random.NextDouble() > 0.1f)
        {
            throw new Exception("Mocked");
        }

        return Task.FromResult<ICollection<Build>>(Array.Empty<Build>());
    }

    public Task SubscribeSseAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileResponse> DownloadBuildAsync(string? buildId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Build> GetBuildSse(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}

public partial class MockAuthenticationClient : IAuthenticationClient
{
    public Task<AuthenticationResult> GetTokenAsync(string? code = null, AuthenticationPlatform? authenticationPlatform = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AuthenticationResult()
        {
            Jwt = string.Empty,
            Username = "fake-user",
            UtcExpiry = DateTime.UtcNow.AddDays(1),
        });
    }
}

public partial class MockEventClient : IEventClient
{
    public Task<ICollection<Event>> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ICollection<Event>>([]);
    }

    public Task SubscribeSseAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<Event> SubscribeSseImpl([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();

        yield break;
    }
}

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

            result.Contributions.Add(new Client.V1.Contribution()
            {
                User = NameGenerator.PersonNames.Get(),
                Id = (i * i + i).ToString(),
                Description = faker.Lorem.Sentences(3),
                CommitDate = DateTime.Now.AddMinutes(-random.Next(1, 60 * 4)),
                Files = [.. files]
            });
        }

        return result;
    }
}