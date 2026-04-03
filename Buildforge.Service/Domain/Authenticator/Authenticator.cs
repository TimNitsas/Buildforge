namespace Buildforge.Service.Domain.Authenticator;

public enum AuthenticationPlatform
{
    None,
    Github
}

public sealed class AuthenticatorResult
{
    public required string UserName { get; set; }
}

public sealed class AuthenticatorOptions
{
    [Required]
    [MinLength(1)]
    public required string SymmetricKey { get; set; }
}

public interface IAuthenticator
{
    Task<AuthenticatorResult> Authenticate(string code);
}