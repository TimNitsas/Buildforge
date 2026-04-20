namespace Buildforge.Service.Provider.Contribution;

public sealed class Contribution
{
    public required string Id { get; init; }

    public required string User { get; init; }

    public required string Description { get; init; }

    public required List<ContributionFile> Files { get; init; }

    public required DateTime CommitDate { get; init; }
}