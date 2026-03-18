using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        var buildResult = await BuildforgeClient.BuildsAsync(0);

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