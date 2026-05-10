namespace Buildforge.Service.Repository.Contribution.V1;

public sealed class ContributionBuild
{
    public required string Id { get; set; }

    public required string Status { get; set; }

    public required string Url { get; set; }

    public required string Branch { get; set; }
}