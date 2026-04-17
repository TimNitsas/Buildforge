namespace Buildforge.Service.Repository.Build.V1;

public sealed class BuildStatusActive : BuildStatus
{
    public required DateTime EstimatedTimeToCompletion { get; init; }
}