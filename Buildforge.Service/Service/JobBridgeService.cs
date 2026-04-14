using Buildforge.Service.Domain.Build.V1;
using Buildforge.Service.Provider.Job;
using RandomFriendlyNameGenerator;

namespace Buildforge.Service.Service;

public class JobBridgeService(IJobProvider jobProvider, BuildRepository buildRepository) : BackgroundService
{
    public override async Task StartAsync(CancellationToken ct)
    {
        await buildRepository.Initialize(ct);

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var semaphore = new SemaphoreSlim(20, 20);

        var work = new List<Task>();

        while (!ct.IsCancellationRequested)
        {
            await foreach (var job in jobProvider.GetJobs(ct))
            {
                work.Add(SynchronizeJob(job, semaphore, ct));
            }

            await Task.Delay(1_000, ct);

            work.RemoveAll(w => w.IsCompleted);
        }

        await Task.WhenAll(work);
    }

    private async Task SynchronizeJob(Job job, SemaphoreSlim semaphore, CancellationToken ct)
    {
        DateTime now = DateTime.UtcNow;

        try
        {
            await semaphore.WaitAsync(ct);

            var build = await buildRepository.GetBuild(job.Id, ct);

            if (build is null)
            {
                var v1 = new Domain.Build.V1.Build()
                {
                    ReadAt = now,
                    Branch = job.Branch,
                    BuildCrashes = [],
                    Contributions = [],
                    Id = job.Id,
                    Name = NameGenerator.Identifiers.Get(),
                    Platform = job.Platform,
                    Status = FromJob(job),
                    Tags = [],
                    Target = job.Target
                };

                await buildRepository.SaveBuild(v1, ct);
            }
            else
            {
                build.ReadAt = now;

                if (build is Domain.Build.V1.Build v1)
                {
                    v1.Status = FromJob(job);

                    await buildRepository.UpdateBuild(v1, ct);
                }
            }
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static BuildStatus FromJob(Job job) => job.Status switch
    {
        JobStatusSuccess s => new BuildStatusActive(),
        JobStatusFailed f => new BuildStatusFailed() { Reason = f.Reason },
        JobStatusQueued q => new BuildStatusQueued(),
        JobStatusActive a => new BuildStatusActive(),
        _ => throw new NotImplementedException()
    };
}