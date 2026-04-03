using Buildforge.App.Domain;
using Buildforge.Client.V1;
using System.Web;

namespace Buildforge.App.ViewModel.Authentication;

public sealed partial class AuthenticationViewModel(IAuthenticationClient client, TokenHandler tokenHandler) : ObservableObject
{
    public string Url => GetUrl();

    public static string RedirectUri => "http://localhost:3000/api/auth/callback/github";

    [ObservableProperty]
    private string? userName;

    public async Task Authenticate(Uri uri)
    {
        var queryString = HttpUtility.ParseQueryString(uri.Query);

        var result = await client.GetTokenAsync(queryString["code"], AuthenticationPlatform.Github);

        await tokenHandler.SaveToken(new Token.V1()
        {
            Payload = result.Jwt,
            UtcExpiry = result.UtcExpiry.Date
        });

        UserName = result.UserName;
    }

    private static string GetUrl()
    {
        var clientId = "Ov23liRaHgpt4Mq5lkkM";
        var scope = "user";
        var state = Guid.NewGuid().ToString();

        StringBuilder sb = new();

        sb.Append("https://github.com/login/oauth/authorize?");
        sb.Append($"client_id={Uri.EscapeDataString(clientId)}");
        sb.Append($"&redirect_uri={Uri.EscapeDataString(RedirectUri)}");
        sb.Append($"&scope={Uri.EscapeDataString(scope)}");
        sb.Append($"&state={Uri.EscapeDataString(state)}");

        return sb.ToString();
    }
}