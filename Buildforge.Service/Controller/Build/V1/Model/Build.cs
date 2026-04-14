namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class Build
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Target { get; init; }

    public required string Platform { get; init; }

    public required string Branch { get; init; }

    public required List<string> Tags { get; init; }

    public required BuildStatus Status { get; init; }

    public required List<BuildContribution> Contributions { get; init; }

    public required List<BuildCrash> Crashes { get; init; }

    public static Build FromDomain(Domain.Build.Build item)
    {
        if (item is Domain.Build.V1.Build v1)
        {
            return new Build()
            {
                Branch = v1.Branch,
                Id = v1.Id,
                Name = v1.Name,
                Platform = v1.Platform,
                Tags = v1.Tags,
                Target = v1.Target,
                Crashes = [.. BuildCrash.FromDomain(v1)],
                Contributions = [.. BuildContribution.FromDomain(v1)],
                Status = BuildStatus.FromDomain(v1),
            };
        }

        throw new NotImplementedException();
    }
}