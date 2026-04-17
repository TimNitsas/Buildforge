namespace Buildforge.Service.Controller.Contribution.V1.Model;

public sealed class ContributionResult
{
    public required List<Contribution> Contributions { get; init; }
}