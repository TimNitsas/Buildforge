using Buildforge.App.Messaging;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Build;

public abstract partial class BuildItemStatusViewModel : ObservableObject, IRecipient<TickTimeMessage>
{
    [ObservableProperty]
    private DateTime startTime;

    protected BuildItemStatusViewModel(Client.V1.BuildStatus status)
    {
        StartTime = status.StartTime.Date;

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(TickTimeMessage message)
    {
        OnPropertyChanged(nameof(StartTime));
    }
}
