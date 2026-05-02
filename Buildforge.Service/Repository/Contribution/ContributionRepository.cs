using Buildforge.Service.Domain.Event.Model;
using Buildforge.Service.Repository.Contribution.V1;
using Buildforge.Service.Repository.Core;
using Npgsql;
using NpgsqlTypes;

namespace Buildforge.Service.Repository.Contribution;

public class ContributionRepository(Database.Database database, EventPublisher eventPublisher)
{
    private readonly string TableName = "contributions";

    private readonly SemaphoreSlim Semaphore = new(1, 1);

    public async IAsyncEnumerable<Contribution> GetContributions([EnumeratorCancellation] CancellationToken ct)
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
                CommitDate = DateTime.UtcNow,
                Builds = new List<ContributionBuild>(),
            };
        }
    }

    public async Task Initialize(CancellationToken ct)
    {
        try
        {
            await Semaphore.WaitAsync(ct);

            using var connection = await database.DataSource.OpenConnectionAsync(ct);

            string sql = @$"
                CREATE TABLE IF NOT EXISTS {TableName} (
                id TEXT PRIMARY KEY,
                data JSONB NOT NULL,
                read_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );";

            await using var cmd = new NpgsqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public async Task SaveContribution(Contribution contribution, CancellationToken ct)
    {
        using var connection = await database.DataSource.OpenConnectionAsync(ct);

        var json = JsonSerializer.Serialize(contribution, Serialization.JsonSerializerOptions);

        string sql = $@"
            INSERT INTO {TableName} (id, data, read_at)
            VALUES (@id, @data, @read_at)
            ON CONFLICT (id)
            DO UPDATE SET data = EXCLUDED.data;
        ";

        await using var cmd = new NpgsqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("id", contribution.Id);
        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, json);
        cmd.Parameters.AddWithValue("read_at", contribution.ReadAt);

        await cmd.ExecuteNonQueryAsync(ct);

        await eventPublisher.Publish(new ContributionCreatedEvent()
        {
            Contribution = contribution,
        });
    }

    public Task WriteLastKnownChange(string lastKnownChange)
    {
        return Task.CompletedTask;
    }

    public Task<string?> GetLastKnownChange(CancellationToken ct)
    {
        return Task.FromResult<string?>(string.Empty);
    }
}