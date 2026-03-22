using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockBuildforgeClient : IBuildforgeClient
{
    private static readonly Random Random = new Random();

    public async Task<BuildResult> BuildsAsync(int? skip = null, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        var buildResult = new BuildResult();

        for (int i = 0; i < 100; i++)
        {
            buildResult.Builds.Add(new Build()
            {
                Name = NameGenerator.Identifiers.Get()
            });
        }

        return buildResult;
    }

    Task<ICollection<BuildStatus>> IBuildforgeClient.UpdatesAsync(CancellationToken cancellationToken)
    {
        if (Random.NextDouble() > 0.5f)
        {
            throw new Exception("Mocked");
        }

        return Task.FromResult<ICollection<BuildStatus>>(Array.Empty<BuildStatus>());
    }
}