using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private BuildItemStatusViewModel? status;

    [ObservableProperty]
    private long? bytes;

    [ObservableProperty]
    private string? target;

    [ObservableProperty]
    private string? platform;

    public ObservableCollection<BuildItemContributionViewModel> Contributions { get; }

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

        Bytes = build.Status switch
        {
            BuildStatusSuccess s => s.Bytes,
            _ => null
        };

        Contributions = new(build.Contributions.Select(c => new BuildItemContributionViewModel(c)));

        Target = build.Target;

        Platform = build.Platform;

        Id = build.Id;
    }
}