using Buildforge.App.Messaging;
using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemViewModel : ObservableObject, IRecipient<TickTimeMessage>
{
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private DateTime? startDate;

    public BuildItemViewModel(Client.V1.Build build)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        Name = build.Name;

        StartDate = build.Status switch
        {
            BuildStatusSuccess s => s.StartTime.UtcDateTime,
            BuildStatusFailed f => f.StartTime.UtcDateTime,
            BuildStatusQueued q => q.StartTime.UtcDateTime,
            _ => null,
        };
    }

    public void Receive(TickTimeMessage message)
    {
        OnPropertyChanged(nameof(StartDate));
    }
}