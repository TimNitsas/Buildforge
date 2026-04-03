using Octokit;

namespace Buildforge.Service.Domain.Authenticator.Github;

public sealed class GithubAuthenticator(IOptions<GithubAuthenticatorOptions> options, IHttpClientFactory factory) : IAuthenticator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection(nameof(GithubAuthenticatorOptions));

        builder.Services.AddOptionsWithValidateOnStart<GithubAuthenticatorOptions>().Bind(section).ValidateDataAnnotations();

        builder.Services.AddHttpClient("Github", o => o.BaseAddress = new Uri("https://github.com")).AddResilienceHandler("Github", o =>
        {
            o.AddConcurrencyLimiter(10);

            o.AddRetry(new HttpRetryStrategyOptions()
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 3
            });
        });

        builder.Services.AddKeyedSingleton<IAuthenticator, GithubAuthenticator>(AuthenticationPlatform.Github.ToString());
    }

    public async Task<AuthenticatorResult> Authenticate(string code)
    {
        HttpClient client = factory.CreateClient("Github");

        var values = new Dictionary<string, string>
        {
            { "client_id", options.Value.ClientId },
            { "client_secret", options.Value.ClientSecret },
            { "code", code },
            { "redirect_uri", options.Value.RedirectUri }
        };

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.PostAsync("https://github.com/login/oauth/access_token", new FormUrlEncodedContent(values));

        var jsonString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<GithubAuthenticatonResult>(jsonString);

        ArgumentNullException.ThrowIfNull(result?.AccessToken);

        var userName = await GetUserLogin(result.AccessToken);

        return new AuthenticatorResult()
        {
            UserName = userName
        };
    }

    private async Task<string> GetUserLogin(string token)
    {
        var client = new GitHubClient(new Octokit.ProductHeaderValue("Buildforge"))
        {
            Credentials = new Credentials(token)
        };

        var user = await client.User.Current();

        return user.Login;
    }
}