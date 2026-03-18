using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel;

public sealed class BuildViewModelDesignTime : BuildViewModel
{
    public BuildViewModelDesignTime() : base(new MockBuildforgeClient())
    {
    }
}
