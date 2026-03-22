namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildStateSuccess : BuildStatus
{
    public required TimeSpan BuildTime { get; init; }
}