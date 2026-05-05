using Buildforge.App.Core.Command.Download;
using Buildforge.App.Domain.Build;
using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject, IRecipient<Event.BuildChangedEvent>, IRecipient<Event.BuildCrashedEvent>, IRecipient<BuildHandlerDownloadProgressEvent>
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

    [ObservableProperty]
    private int crashCount;

    [ObservableProperty]
    private string? progressText;

    [ObservableProperty]
    private double? progressValue;

    [ObservableProperty]
    private TimeSpan? meanTimeToCrash;

    public List<Client.V1.BuildCrash> Crashes { get; } = new();

    public BuildItemContributionViewModel Contributions { get; }

    public ICommand DownloadCommand { get; }

    public string? Key => Id;

    public BuildItemViewModel(Client.V1.Build build)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        Contributions = new(build.Contributions);

        Name = build.Name;

        Target = build.Target;

        Platform = build.Platform;

        Id = build.Id;

        Patch(build);

        DownloadCommand = new DownloadBuildCommand(build.Id);
    }

    private void Patch(Client.V1.Build build)
    {
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

        foreach (var crash in build.Crashes)
        {
            HandleCrash(crash);
        }
    }

    public void Receive(Event.BuildCrashedEvent message)
    {
        HandleCrash(message.Inner);
    }

    private void HandleCrash(Client.V1.BuildCrash buildCrash)
    {
        if (!buildCrash.BuildId.Equals(Id))
        {
            return;
        }

        Crashes.Add(buildCrash);

        CrashCount = Crashes.Count;

        MeanTimeToCrash = TimeSpan.FromSeconds(Crashes.Average(c => c.PlayTime.TotalSeconds));
    }

    public void Receive(Event.BuildChangedEvent message)
    {
        if (!message.Inner.Id.Equals(Id))
        {
            return;
        }

        Patch(message.Inner);
    }

    public void Receive(BuildHandlerDownloadProgressEvent message)
    {
        if (!message.BuildId.Equals(Id))
        {
            return;
        }

        ProgressValue = message.ProgressPercentage;

        ProgressText = $"{message.SpeedKbps} ({message.EstimatedEta})";
    }
}