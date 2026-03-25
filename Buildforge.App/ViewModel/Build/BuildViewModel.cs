using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel;

public partial class BuildViewModel : ObservableObject
{
    public ObservableCollection<BuildItemViewModel> Builds { get; } = [];

    private readonly IBuildforgeClient BuildforgeClient;

    public BuildViewModel(IBuildforgeClient client)
    {
        BuildforgeClient = client;
    }

    [RelayCommand]
    public async Task Load()
    {
        var buildResult = await BuildforgeClient.GetBuildAsync(0);

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