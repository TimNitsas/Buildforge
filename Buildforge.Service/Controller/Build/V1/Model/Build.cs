namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class Build
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Target { get; init; }

    public required string Platform { get; init; }

    public required string Branch { get; init; }

    public required BuildStatus Status { get; init; }

    public required List<BuildContribution> Contributions { get; init; }

    public required List<BuildCrash> Crashes { get; init; }

    public required List<string> Tags { get; init; }
}