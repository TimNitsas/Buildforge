namespace Buildforge.Service.Provider.Job;

public sealed class JobStatusActive : JobStatus
{
    public required DateTime EstimatedTimeToCompletion { get; init; }
}