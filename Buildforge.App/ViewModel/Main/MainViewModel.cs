namespace Buildforge.App.ViewModel.Main;

public partial class MainViewModel
{
    public string? Version { get; }

    public MainViewModel()
    {
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    }
}

public sealed class MainViewModelDesignTime : MainViewModel
{

}