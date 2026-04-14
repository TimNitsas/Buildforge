namespace Buildforge.Service.Provider.Contribution;

public interface IContributionProvider
{
    IAsyncEnumerable<Contribution> GetContributions(CancellationToken ct);
}

public sealed class ContributionProvider : IContributionProvider
{
    public IAsyncEnumerable<Contribution> GetContributions(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}