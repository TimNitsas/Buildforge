using Buildforge.App.Core.Command.Shared;
using Buildforge.App.Domain.Build;

namespace Buildforge.App.Core.Command.Download;

public sealed class DownloadBuildCommand(string buildId) : SharedCommand($"download-build-{buildId}")
{
    protected override bool CanExecuteImpl()
    {
        return !App.Services.GetRequiredService<BuildLocator>().HasBuild(buildId);
    }

    public override Task ExecuteImpl()
    {
        return App.Services.GetRequiredService<BuildHandler>().DownloadBuild(buildId, Cts.Token);
    }
}