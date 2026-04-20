using Buildforge.Service.Repository.Contribution.V1;
using Npgsql;

namespace Buildforge.Service.Repository.Contribution;

public class ContributionRepository(Database.Database database)
{
    internal async IAsyncEnumerable<Contribution> GetContributions([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();

        for (int i = 0; i < 10; i++)
        {
            yield return new V1.Contribution()
            {
                ReadAt = DateTime.Now,
                Description = i.ToString(),
                Files = new List<ContributionFile>(),
                Id = i.ToString(),
                User = i.ToString(),
                CommitDate = DateTime.UtcNow
            };
        }
    }
}