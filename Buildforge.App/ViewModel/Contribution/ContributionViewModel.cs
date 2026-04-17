using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Contribution;

public partial class ContributionViewModel(IContributionClient client) : ObservableObject
{
    public ObservableCollection<ContributionItemViewModel> Contributions { get; } = [];

    [RelayCommand]
    public async Task Load()
    {
        var result = await client.GetContributionsAsync(0);

        if (result is null)
        {
            return;
        }

        foreach (var item in result.Contributions)
        {
            Contributions.Add(new ContributionItemViewModel(item));
        }
    }
}