using ApiSdk.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Buildforge.App.ViewModel;

public partial class BuildItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string? name;

    public BuildItemViewModel(Build build)
    {
        Name = build.Name;
    }
}