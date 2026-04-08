namespace Buildforge.App.Domain.Build;

public sealed class BuildLocator(string rootFolder)
{
    public string RootFolder { get; } = rootFolder;

    public bool HasBuild(string buildId)
    {
        return File.Exists(Path.Combine(RootFolder, $"{buildId}.bin"));
    }
}