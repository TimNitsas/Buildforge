namespace Buildforge.App.ViewModel;

public partial class MainViewModel
{
    public string? Version { get; }

    public MainViewModel()
    {
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    }
}