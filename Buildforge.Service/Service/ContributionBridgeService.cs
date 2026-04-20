using Buildforge.Service.Provider.Contribution;
using Buildforge.Service.Repository.Contribution;

namespace Buildforge.Service.Service;

public sealed class ContributionBridgeService(IContributionProvider contributionProvider, ContributionRepository contributionRepository) : BackgroundService
{
    public override async Task StartAsync(CancellationToken ct)
    {
        await contributionRepository.Initialize(ct);

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var data = await contributionProvider.GetContributions(ct).ToListAsync();
    }
}