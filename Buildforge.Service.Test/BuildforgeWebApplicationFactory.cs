namespace Buildforge.Service.Test;

public class BuildforgeWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private const string Scheme = "Test";

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(Scheme).AddScheme<AuthenticationSchemeOptions, BuildforgeAuthenticationHandler>(Scheme, options => { });
        });
    }
}
