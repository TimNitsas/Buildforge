using Buildforge.App.Domain.Token;
using Buildforge.App.Event;
using Buildforge.App.ViewModel.Authentication;
using Buildforge.App.ViewModel.Main;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Root;

public partial class RootViewModel : ObservableObject, IRecipient<TokenAcquiredEvent>
{
    private readonly TokenHandler TokenHandler;

    [ObservableProperty]
    private object? currentViewModel;

    public RootViewModel(TokenHandler tokenHandler)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        TokenHandler = tokenHandler;

        CurrentViewModel = GetCurrentViewModel(tokenHandler);
    }

    public void Receive(TokenAcquiredEvent message)
    {
        CurrentViewModel = GetCurrentViewModel(TokenHandler);
    }

    private static object GetCurrentViewModel(TokenHandler tokenHandler)
    {
        return tokenHandler.HasValidToken() switch
        {
            true => App.Services.GetRequiredService<MainViewModel>(),
            false => App.Services.GetRequiredService<AuthenticationViewModel>()
        };
    }
}