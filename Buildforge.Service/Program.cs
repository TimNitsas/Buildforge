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

        builder.Services.AddControllers();

        builder.Services.AddOpenApiDocument(s =>
        {
            s.ApiGroupNames = ["v1"];
            s.Title = "Buildforge Service";
        });

        var app = builder.Build();

        app.MapControllers();

        app.UseOpenApi();

        app.UseSwaggerUi();

        app.Run();
    }
}