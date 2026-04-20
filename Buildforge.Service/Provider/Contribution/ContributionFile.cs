namespace Buildforge.Service.Provider.Contribution;

public sealed class ContributionFile
{
    public required string DepotPath { get; init; }

    public required long? Size { get; init; }

    public required ContributionAction? ContributionAction { get; init; }

    public required string Hash { get; init; }
}