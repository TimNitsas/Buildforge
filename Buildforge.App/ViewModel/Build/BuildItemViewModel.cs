using Buildforge.App.Core.Command.Download;
using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject, IRecipient<Event.BuildChangedEvent>, IRecipient<Event.BuildCrashedEvent>
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

    public ObservableCollection<BuildItemContributionViewModel> Contributions { get; }

    public ICommand DownloadCommand { get; }

    public BuildItemViewModel(Client.V1.Build build)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        Contributions = new(build.Contributions.Select(c => new BuildItemContributionViewModel(c)));

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
    }

    public void Receive(Event.BuildCrashedEvent message)
    {
        if (!message.Inner.BuildId.Equals(Id))
        {
            return;
        }

        CrashCount++;
    }

    public void Receive(Event.BuildChangedEvent message)
    {
        if (!message.Inner.Id.Equals(Id))
        {
            return;
        }

        Patch(message.Inner);
    }
}