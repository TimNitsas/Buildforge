namespace Buildforge.App.ViewModel.Contribution;

public sealed partial class ContributionItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string id;

    [ObservableProperty]
    private string user;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    private DateTime commitDate;

    [ObservableProperty]
    private int fileCount;

    [ObservableProperty]
    private long fileSize;

    public ObservableCollection<ContributionItemBuildViewModel> Builds { get; } = [];

    public string Key => Id;

    public ContributionItemViewModel(Client.V1.Contribution item)
    {
        User = item.User;

        Description = item.Description;

        Id = item.Id;

        CommitDate = item.CommitDate.Date;

        FileCount = item.Files.Count;

        FileSize = item.Files.Sum(f => f.Size);

        foreach (var build in item.Builds)
        {
            Builds.Add(new ContributionItemBuildViewModel(build));
        }
    }
}