namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildResult
{
    public required List<Build> Builds { get; init; }
}