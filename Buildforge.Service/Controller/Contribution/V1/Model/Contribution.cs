namespace Buildforge.Service.Controller.Contribution.V1.Model;

public sealed class Contribution
{
    public required string Id { get; init; }

    public required DateTime CommitDate { get; init; }

    public required string User { get; init; }

    public required string Description { get; init; }

    public required List<ContributionFile> Files { get; set; }

    public required List<ContributionBuild> Builds { get; init; }

    public static Contribution FromDomain(Repository.Contribution.Contribution c)
    {
        if (c is Repository.Contribution.V1.Contribution v1)
        {
            var files = v1.Files.Select(f => new ContributionFile
            {
                Path = f.Path,
                Size = f.Size ?? 0
            });

            var builds = v1.Builds.Select(b => new ContributionBuild()
            {
                Id = b.Id,
                Status = b.Status,
                Url = b.Url,
                Branch = b.Branch,
            });

            return new Contribution()
            {
                Description = v1.Description,
                Id = v1.Id,
                User = v1.User,
                CommitDate = v1.CommitDate,
                Files = [.. files],
                Builds = [.. builds]
            };
        }

        throw new NotImplementedException();
    }
}