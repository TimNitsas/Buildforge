using Buildforge.App.Messaging;
using Humanizer;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemStatusActiveViewModel : BuildItemStatusViewModel
{
    [ObservableProperty]
    private double completionValue;

    [ObservableProperty]
    private string? completionText;

    private readonly Client.V1.BuildStatusActive Status;

    public BuildItemStatusActiveViewModel(Client.V1.BuildStatusActive status) : base(status)
    {
        Status = status;

        ComputeProgress(status);
    }

    private void ComputeProgress(Client.V1.BuildStatusActive status)
    {
        var duration = status.EstimatedTimeToCompletion - status.StartTime;

        var elapsed = DateTime.UtcNow - status.StartTime;

        CompletionValue = 100 * elapsed.TotalSeconds / duration.TotalSeconds;

        CompletionText = $"{(elapsed - duration).Humanize()} left";
    }

    protected override void OnReceive(TickTimeRefreshEvent message)
    {
        ComputeProgress(Status);
    }
}