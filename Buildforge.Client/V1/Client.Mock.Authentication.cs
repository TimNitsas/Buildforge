namespace Buildforge.Client.V1;

public partial class MockAuthenticationClient : IAuthenticationClient
{
    public Task<AuthenticationResult> GetTokenAsync(string? code = null, AuthenticationPlatform? authenticationPlatform = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AuthenticationResult()
        {
            Jwt = string.Empty,
            Username = "fake-user",
            UtcExpiry = DateTime.UtcNow.AddDays(1),
        });
    }
}