using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockBuildforgeClient : IBuildforgeClient
{
    private static readonly Random Random = new Random();

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
                Name = NameGenerator.Identifiers.Get(),
                Status = BuildStatus[Random.Next(BuildStatus.Count)]()
            });
        }

        return buildResult;
    }

    private static readonly List<Func<BuildStatus>> BuildStatus =
    [
        () => new BuildStatusFailed(),
        () => new BuildStatusQueued(),
        () => new BuildStatusActive(),
        () => new BuildStatusSuccess()
        {
            BuildTime = TimeSpan.FromMinutes(Random.Next(1, 240)),
            StartTime = DateTime.UtcNow - TimeSpan.FromSeconds(Random.Next(240, 480))
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
}