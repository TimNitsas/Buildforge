namespace Buildforge.Service.Controller.Build.V1.Model;

public sealed class BuildCrash
{
    public required string BuildId { get; init; }

    public required string User { get; init; }

    public required DateTime Date { get; init; }

    public required TimeSpan PlayTime { get; init; }

    public static IEnumerable<BuildCrash> FromDomain(Repository.Build.V1.Build item)
    {
        return item.BuildCrashes.Select(c => new BuildCrash()
        {
            Date = c.Date,
            PlayTime = c.PlayTime,
            User = c.User,
            BuildId = item.Id,
        });
    }
}