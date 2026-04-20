namespace Buildforge.Service.Provider.Contribution;

public interface IContributionProvider
{
    IAsyncEnumerable<Contribution> GetContributions(object? startAtKey, CancellationToken ct);
}