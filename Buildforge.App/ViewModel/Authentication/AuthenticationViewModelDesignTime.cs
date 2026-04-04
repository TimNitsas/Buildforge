#if DEBUG
using Buildforge.App.Domain;
using Buildforge.Client.V1;

namespace Buildforge.App.ViewModel.Authentication;

public sealed class AuthenticationViewModelDesignTime : AuthenticationViewModel
{
    public AuthenticationViewModelDesignTime() : base(new Moq.Mock<IAuthenticationClient>().Object, new TokenHandler())
    {
    }
}
#endif