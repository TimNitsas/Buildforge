namespace Buildforge.Service.Provider.Job;

public sealed class JobStatusFailed : JobStatus
{
    public required string Reason { get; init; }
}