namespace Buildforge.Service.Domain.Build.V1;

public sealed class BuildStatusSuccess : BuildStatus
{
    public required TimeSpan BuildTime { get; init; }

    public required long Bytes { get; init; }
}