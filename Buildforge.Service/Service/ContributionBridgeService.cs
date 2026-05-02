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
        var start = await contributionRepository.GetLastKnownChange(ct);

        while (!ct.IsCancellationRequested)
        {
            await foreach (var item in contributionProvider.GetContributions(start, ct))
            {
                var files = item.Files.Select(f => new Repository.Contribution.V1.ContributionFile()
                {
                    Path = f.DepotPath,
                    Size = f.Size,
                });

                Repository.Contribution.V1.Contribution contribution = new()
                {
                    CommitDate = item.CommitDate,
                    Description = item.Description,
                    Id = item.Id,
                    ReadAt = DateTime.UtcNow,
                    User = item.User,
                    Files = [.. files],
                    Builds = []
                };

                await contributionRepository.SaveContribution(contribution, ct);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), ct);
        }
    }
}