using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private DateTime? startDate;

    public BuildItemViewModel(Client.V1.Build build)
    {
        Name = build.Name;

        StartDate = build.Status switch
        {
            BuildStatusSuccess s => s.StartTime.UtcDateTime,
            BuildStatusFailed f => f.StartTime.UtcDateTime,
            BuildStatusQueued q => q.StartTime.UtcDateTime,
            _ => null,
        };
    }
}