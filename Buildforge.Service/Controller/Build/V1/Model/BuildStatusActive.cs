namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildStatusActive : BuildStatus
{
    public required DateTime EstimatedTimeToCompletion { get; init; }
}