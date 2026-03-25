namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemStatusFailedViewModel : BuildItemStatusViewModel
{
    [ObservableProperty]
    private string? reason;

    public BuildItemStatusFailedViewModel(Client.V1.BuildStatusFailed status) : base(status)
    {
        Reason = status.Reason;
    }
}
