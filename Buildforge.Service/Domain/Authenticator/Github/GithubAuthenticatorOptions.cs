using System.ComponentModel.DataAnnotations;

namespace Buildforge.Service.Domain.Authenticator.Github;

public class GithubAuthenticatorOptions
{
    [Required]
    [MinLength(1)]
    public required string ClientSecret { get; set; }

    [Required]
    [MinLength(1)]
    public required string RedirectUri { get; set; }

    [Required]
    [MinLength(1)]
    public required string ClientId { get; set; }
}