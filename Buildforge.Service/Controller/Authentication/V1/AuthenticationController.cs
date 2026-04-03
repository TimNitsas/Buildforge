using Buildforge.Service.Domain.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Buildforge.Service.Controller.Authentication.V1;

[ApiController]
[Route("api/v1/authentication")]
[ApiExplorerSettings(GroupName = "v1")]
public sealed class AuthenticationController(IOptions<AuthenticatorOptions> options, ILogger<AuthenticationController> logger) : ControllerBase
{
    public class GetCodeQueryParameters
    {
        public string Code { get; set; } = string.Empty;

        public AuthenticationPlatform AuthenticationPlatform { get; set; }
    }

    [HttpGet()]
    [AllowAnonymous]
    public async Task<AuthenticationResult> GetToken([FromQuery] GetCodeQueryParameters query)
    {
        var authenticator = HttpContext.RequestServices.GetRequiredKeyedService<IAuthenticator>(query.AuthenticationPlatform.ToString());

        AuthenticatorResult authenticationResult = await authenticator.Authenticate(query.Code);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SymmetricKey));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var expiry = now.AddHours(1);

        var id = Guid.NewGuid().ToString();

        var token = new JwtSecurityToken
        (
            signingCredentials: signingCredentials,
            issuer: "buildforge.service",
            audience: "user",
            expires: expiry,
            notBefore: now,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, authenticationResult.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, id),
            ]
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        logger.LogInformation("Jwt {id} issued for {userName}", id, authenticationResult.UserName);

        return new AuthenticationResult()
        {
            Jwt = jwt,
            UtcExpiry = expiry,
            Username = authenticationResult.UserName
        };
    }
}