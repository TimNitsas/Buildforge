namespace Buildforge.Service.Domain.Build.V1;

public sealed class Build : Domain.Build.Build
{
    public required string Name { get; set; }

    public required string Target { get; set; }

    public required string Platform { get; set; }

    public required string Branch { get; set; }

    public required List<string> Tags { get; set; }

    public required List<BuildContribution> Contributions { get; set; }

    public required List<BuildCrash> BuildCrashes { get; set; }

    public required BuildStatus Status { get; set; }
}

public sealed class BuildCrash
{
    public required string User { get; init; }

    public required DateTime Date { get; init; }

    public required TimeSpan PlayTime { get; init; }
}

public sealed class BuildContribution
{
    public required string Id { get; init; }

    public required string User { get; init; }

    public required string Description { get; init; }
}