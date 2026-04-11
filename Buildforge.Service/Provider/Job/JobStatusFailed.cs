namespace Buildforge.Service.Provider.Job;

public sealed class JobStatusFailed : BuildStatus
{
    public required string Reason { get; init; }
}