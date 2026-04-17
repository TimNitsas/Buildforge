using Humanizer;

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
    private int fileSize;

    public ContributionItemViewModel(Client.V1.Contribution item)
    {
        User = item.User;

        Description = item.Description;

        Id = item.Id;

        CommitDate = item.CommitDate.Date;

        FileCount = item.Files.Count;

        FileSize = item.Files.Sum(f => f.Size);
    }
}