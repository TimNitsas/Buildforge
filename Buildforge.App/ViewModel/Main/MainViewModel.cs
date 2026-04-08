using Buildforge.App.Domain.Token;
using Buildforge.App.Event;
using Buildforge.App.ViewModel.Authentication;
using Buildforge.App.ViewModel.Contribution;
using CommunityToolkit.Mvvm.Messaging;

namespace Buildforge.App.ViewModel.Main;

public partial class MainViewModel : ObservableObject, IRecipient<TokenAcquiredEvent>
{
    [ObservableProperty]
    private string? version;

    [ObservableProperty]
    private object? currentViewModel;

    [ObservableProperty]
    private string? username;

    private readonly TokenHandler TokenHandler;

    public MainViewModel(TokenHandler tokenHandler)
    {
        TokenHandler = tokenHandler;

        WeakReferenceMessenger.Default.RegisterAll(this);

        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        CurrentViewModel = GetCurrentViewModel(TokenHandler);

        if (tokenHandler.TryLoadToken(out var token))
        {
            if (token is Token.V1 v1)
            {
                Username = v1.Username;
            }
        }
    }

    private void SetMainViewModel<T>() where T : notnull
    {
        CurrentViewModel = App.Services.GetRequiredService<T>();
    }

    [RelayCommand]
    private void ShowBuilds()
    {
        SetMainViewModel<BuildViewModel>();
    }

    [RelayCommand]
    private void ShowContributions()
    {
        SetMainViewModel<ContributionViewModel>();
    }

    public void Receive(TokenAcquiredEvent message)
    {
        Username = message.Username;

        CurrentViewModel = GetCurrentViewModel(TokenHandler);
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