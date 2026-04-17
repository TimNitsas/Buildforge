using Buildforge.Service.Database;
using Buildforge.Service.Domain.Authenticator;
using Buildforge.Service.Domain.Authenticator.Github;
using Buildforge.Service.Provider.Contribution;
using Buildforge.Service.Provider.Crash;
using Buildforge.Service.Provider.Crash.Simulation;
using Buildforge.Service.Provider.Job;
using Buildforge.Service.Provider.Job.Simulation;
using Buildforge.Service.Repository.Build;
using Buildforge.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Buildforge.Service;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var authenticatorSection = builder.Configuration.GetSection(nameof(AuthenticatorOptions));

        builder.Services.AddOptionsWithValidateOnStart<AuthenticatorOptions>().Bind(authenticatorSection).ValidateDataAnnotations();

        builder.Services.AddOptionsWithValidateOnStart<DatabaseOptions>().Bind(builder.Configuration.GetSection(nameof(DatabaseOptions))).ValidateDataAnnotations();

        builder.Services.AddControllers();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            var key = authenticatorSection.Get<AuthenticatorOptions>()!.SymmetricKey;

            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        RegisterBackgroundServices(builder);

        builder.Services.AddSingleton<BuildRepository>();

        builder.Services.AddSingleton<ICrashProvider, CrashProviderSimulator>();

        builder.Services.AddSingleton<IContributionProvider, ContributionProvider>();

        builder.Services.AddSingleton<EventPublisher>();

        builder.Services.AddSingleton<Database.Database>();

        builder.Services.AddSingleton<IJobProvider>(new JobProviderSimulator(100));

        GithubAuthenticator.Configure(builder);

        builder.Services.AddOpenApiDocument(s =>
        {
            s.ApiGroupNames = ["v1"];

            s.Title = "Buildforge Service";

            s.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });

            s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(JwtBearerDefaults.AuthenticationScheme));
        });

        var app = builder.Build();

        app.MapControllers();

        app.UseOpenApi();

        app.UseSwaggerUi();

        app.Run();
    }

    private static void RegisterBackgroundServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<JobBridgeService>();

        builder.Services.AddHostedService<ContributionBridgeService>();

        builder.Services.AddHostedService<CrashBridgeService>();
    }
}