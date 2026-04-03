namespace Buildforge.Service.Controller.Authentication.V1;

public sealed class AuthenticationResult
{
    public required string Jwt { get; set; }

    public required string UserName { get; set; }

    public DateTime UtcExpiry { get; set; }
}