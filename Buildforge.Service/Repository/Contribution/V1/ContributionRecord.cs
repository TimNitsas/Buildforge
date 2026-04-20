namespace Buildforge.Service.Repository.Contribution.V1;

public sealed class ContributionRecord : Repository.Contribution.ContributionRecord
{
    public required string LastKnownChange { get; set; }
}