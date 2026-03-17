using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Buildforge.App.ViewModel;

public partial class BuildViewModel : ObservableObject
{
    public ObservableCollection<BuildItemViewModel> Builds { get; } = [];
}
