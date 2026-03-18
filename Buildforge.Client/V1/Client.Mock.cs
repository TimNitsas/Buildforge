using RandomFriendlyNameGenerator;

namespace Buildforge.Client.V1;

public partial class MockBuildforgeClient : IBuildforgeClient
{
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
}