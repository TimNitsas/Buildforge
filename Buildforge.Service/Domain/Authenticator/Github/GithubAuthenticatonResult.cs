using System.Text.Json.Serialization;

namespace Buildforge.Service.Domain.Authenticator.Github;

public sealed class GithubAuthenticatonResult
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
}
