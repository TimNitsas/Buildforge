namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildContribution
{
    public required string Id { get; init; }

    public required string User { get; init; }

    public required string Description { get; init; }

    public static IEnumerable<BuildContribution> FromDomain(Domain.Build.V1.Build item)
    {
        foreach (var contribution in item.Contributions)
        {
            yield return new BuildContribution()
            {
                Description = contribution.Description,
                Id = contribution.Id,
                User = contribution.Id
            };
        }
    }
}