namespace Buildforge.Service.Repository.Contribution.V1;

public sealed class Contribution : Repository.Contribution.Contribution
{
    public required string User { get; init; }

    public required string Description { get; init; }

    public required DateTime CommitDate { get; set; }

    public required List<ContributionFile> Files { get; set; }
}