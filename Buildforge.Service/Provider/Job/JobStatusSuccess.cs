namespace Buildforge.Service.Provider.Job;

public sealed class JobStatusSuccess : BuildStatus
{
    public required TimeSpan BuildTime { get; init; }

    public required long Bytes { get; init; }
}