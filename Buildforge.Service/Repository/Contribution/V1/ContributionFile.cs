namespace Buildforge.Service.Repository.Contribution.V1;

public sealed class ContributionFile
{
    public required string Path { get; set; }

    public required long? Size { get; set; }
}