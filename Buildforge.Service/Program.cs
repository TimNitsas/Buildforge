namespace Buildforge.Service;

public class Program
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