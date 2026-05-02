namespace Buildforge.Service.Provider.Job.Simulation;

public sealed class JobProviderSimulator : IJobProvider
{
    private readonly List<JobProviderSimulatorItem> Jobs = [];

    public JobProviderSimulator(int buildCount)
    {
        for (int i = 0; i < buildCount; i++)
        {
            Jobs.Add(new JobProviderSimulatorItem(i));
        }
    }

    public async IAsyncEnumerable<Job> GetJobs([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();

        foreach (var job in Jobs)
        {
            yield return job.Resolve();
        }
    }
}