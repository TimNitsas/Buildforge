namespace Buildforge.App.ViewModel.Build;

public partial class BuildItemStatusSuccessViewModel : BuildItemStatusViewModel
{
    [ObservableProperty]
    private TimeSpan? buildTime;

    public BuildItemStatusSuccessViewModel(Client.V1.BuildStatusSuccess status) : base(status)
    {
        BuildTime = status.BuildTime;
    }
}
