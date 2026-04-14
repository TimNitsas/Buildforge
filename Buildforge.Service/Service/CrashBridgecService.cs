using Buildforge.Service.Domain.Event.Model;
using Buildforge.Service.Provider.Crash;

namespace Buildforge.Service.Service;

public sealed class CrashBridgeService(ICrashProvider crashProvider, EventPublisher eventPublisher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            foreach (var crash in crashProvider.GetCrashes(ct))
            {
                await eventPublisher.Publish(new BuildCrashedEvent()
                {
                    Data = crash
                });
            }

            await Task.Delay(1_000, ct);
        }
    }
}