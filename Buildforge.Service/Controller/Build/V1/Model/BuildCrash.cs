namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildCrash
{
    public required string User { get; init; }

    public required DateTime Date { get; init; }

    public required TimeSpan PlayTime { get; init; }
}