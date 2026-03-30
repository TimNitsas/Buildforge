namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildContribution
{
    public required string Id { get; init; }

    public required string User { get; init; }

    public required string Description { get; init; }
}