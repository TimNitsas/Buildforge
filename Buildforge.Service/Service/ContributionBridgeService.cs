using Buildforge.Service.Provider.Contribution;

namespace Buildforge.Service.Service;

public sealed class ContributionBridgeService(IContributionProvider contributionProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var data = await contributionProvider.GetContributions(ct).ToListAsync();
    }
}