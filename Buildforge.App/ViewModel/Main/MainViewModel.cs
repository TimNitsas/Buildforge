using Buildforge.App.Domain;
using Buildforge.App.Event;
using Buildforge.App.ViewModel.Authentication;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Main;

public partial class MainViewModel : ObservableObject, IRecipient<TokenAcquiredEvent>
{
    public string? Version { get; }

    public object CurrentViewModel => GetCurrentViewModel(TokenHandler);

    private readonly TokenHandler TokenHandler;

    public MainViewModel(TokenHandler tokenHandler)
    {
        TokenHandler = tokenHandler;

        WeakReferenceMessenger.Default.RegisterAll(this);

        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public void Receive(TokenAcquiredEvent message)
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private static object GetCurrentViewModel(TokenHandler tokenHandler)
    {
        return tokenHandler.HasValidToken() switch
        {
            true => App.Services.GetRequiredService<BuildViewModel>(),
            false => App.Services.GetRequiredService<AuthenticationViewModel>()
        };
    }
}