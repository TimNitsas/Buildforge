using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private BuildItemStatusViewModel? status;

    public BuildItemViewModel(Client.V1.Build build)
    {
        Name = build.Name;

        Status = build.Status switch
        {
            BuildStatusSuccess s => new BuildItemStatusSuccessViewModel(s),
            BuildStatusFailed f => new BuildItemStatusFailedViewModel(f),
            BuildStatusQueued q => new BuildItemStatusQueuedViewModel(q),
            BuildStatusActive a => new BuildItemStatusActiveViewModel(a),
            _ => null
        };
    }
}