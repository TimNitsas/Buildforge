using Scalar.AspNetCore;

namespace Buildforge.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapOpenApi();

        app.MapScalarApiReference();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}