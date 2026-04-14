namespace Buildforge.Service.Domain.Build.V1;

public sealed class BuildStatusFailed : BuildStatus
{
    public required string Reason { get; init; }
}