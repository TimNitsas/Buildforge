namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildStatusFailed : BuildStatus
{
    public required string Reason { get; init; }
}
