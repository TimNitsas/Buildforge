using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Build;

public partial class BuildViewModel(IBuildClient client) : ObservableObject
{
    public ObservableCollection<BuildItemViewModel> Builds { get; } = [];

    [RelayCommand]
    public async Task Load()
    {
        var buildResult = await client.GetBuildsAsync(0);

        if (buildResult is null)
        {
            return;
        }

        foreach (var item in buildResult.Builds)
        {
            Builds.Add(new BuildItemViewModel(item));
        }
    }
}