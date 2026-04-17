namespace Buildforge.Service.Controller.Contribution.V1.Model;

public sealed class ContributionFile
{
    public required string Path { get; set; }

    public required int Size { get; set; }
}