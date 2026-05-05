namespace Buildforge.App.ViewModel.Build;

public class BuildItemContributionViewModel : ObservableObject
{
    public ObservableCollection<BuildItemContributionItemViewModel> Items { get; } = [];

    public BuildItemContributionViewModel(IEnumerable<Client.V1.BuildContribution> items)
    {
        foreach (var item in items)
        {
            Items.Add(new BuildItemContributionItemViewModel(item));
        }
    }
}