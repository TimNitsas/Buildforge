namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class Build
{
    public required string Name { get; init; }

    public required string Target { get; init; }

    public required BuildStatus Status { get; init; }
}