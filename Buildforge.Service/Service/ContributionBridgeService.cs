using Buildforge.Service.Provider.Contribution;
using Buildforge.Service.Repository.Contribution;

namespace Buildforge.Service.Service;

public sealed class ContributionBridgeService(IContributionProvider contributionProvider, ContributionRepository contributionRepository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var data = await contributionProvider.GetContributions(ct).ToListAsync();
    }
}