namespace Buildforge.Service.Provider.Job;

public interface IJobProvider
{
    IAsyncEnumerable<Job> GetJobs(CancellationToken ct);
}