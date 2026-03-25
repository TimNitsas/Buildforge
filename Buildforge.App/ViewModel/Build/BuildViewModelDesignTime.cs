using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Build;

public sealed class BuildViewModelDesignTime : BuildViewModel
{
    public BuildViewModelDesignTime() : base(new MockBuildforgeClient())
    {
        foreach (var build in MockBuildforgeClient.GetBuildResult().Builds)
        {
            Builds.Add(new BuildItemViewModel(build));
        }
    }
}