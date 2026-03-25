namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemStatusQueuedViewModel : BuildItemStatusViewModel
{
    public BuildItemStatusQueuedViewModel(Client.V1.BuildStatusQueued status) : base(status)
    {
    }
}
