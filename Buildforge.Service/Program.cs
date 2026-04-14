using Buildforge.Service.Domain.Authenticator;
using Buildforge.Service.Domain.Authenticator.Github;
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
}