using Buildforge.Service.Domain.Event.Model;
using Npgsql;
using NpgsqlTypes;

namespace Buildforge.Service.Domain.Build;

public sealed class BuildRepository(Database.Database database, EventPublisher eventPublisher)
{
    private readonly SemaphoreSlim Semaphore = new(1, 1);

    private readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        AllowOutOfOrderMetadataProperties = true
    };

    public async Task Initialize(CancellationToken ct)
    {
        try
        {
            await Semaphore.WaitAsync(ct);

            using var connection = await database.DataSource.OpenConnectionAsync(ct);

            const string sql = @"
                CREATE TABLE IF NOT EXISTS builds (
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

    public async IAsyncEnumerable<Build> GetBuilds([EnumeratorCancellation] CancellationToken ct)
    {
        using var connection = await database.DataSource.OpenConnectionAsync(ct);

        const string sql = @"SELECT data FROM builds";

        await using var cmd = new NpgsqlCommand(sql, connection);

        await using var reader = await cmd.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            var json = reader.GetString(0);

            var build = JsonSerializer.Deserialize<Build>(json, JsonSerializerOptions);

            if (build != null)
            {
                yield return build;
            }
        }
    }

    public async Task SaveBuild(Build build, CancellationToken ct)
    {
        using var connection = await database.DataSource.OpenConnectionAsync(ct);

        var json = JsonSerializer.Serialize(build, JsonSerializerOptions);

        const string sql = @"
            INSERT INTO builds (id, data, read_at)
            VALUES (@id, @data, @read_at)
            ON CONFLICT (id)
            DO UPDATE SET data = EXCLUDED.data;
        ";

        await using var cmd = new NpgsqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("id", build.Id);
        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, json);
        cmd.Parameters.AddWithValue("read_at", build.ReadAt);

        await cmd.ExecuteNonQueryAsync(ct);

        await eventPublisher.Publish(new BuildCreatedEvent()
        {
            Build = build
        });
    }

    public async Task UpdateBuild(Build build, CancellationToken ct)
    {
        using var connection = await database.DataSource.OpenConnectionAsync(ct);

        var json = JsonSerializer.Serialize(build, JsonSerializerOptions);

        const string sql = @"
            UPDATE builds
            SET data = @data::jsonb, read_at = @read_at
            WHERE id = @id
            AND read_at <= @read_at;
        ";

        await using var cmd = new NpgsqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("id", build.Id);
        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, json);
        cmd.Parameters.AddWithValue("read_at", build.ReadAt);

        var changes = await cmd.ExecuteNonQueryAsync(ct);

        if (changes > 0)
        {
            await eventPublisher.Publish(new BuildChangedEvent()
            {
                Build = build
            });
        }
    }

    public async Task<Build?> GetBuild(string buildId, CancellationToken ct)
    {
        using var connection = await database.DataSource.OpenConnectionAsync(ct);

        const string sql = @"SELECT data FROM builds WHERE id = @id";

        await using var cmd = new NpgsqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("id", buildId);

        await using var reader = await cmd.ExecuteReaderAsync(ct);

        if (!await reader.ReadAsync(ct))
        {
            return null;
        }

        var json = reader.GetString(0);

        return JsonSerializer.Deserialize<Build>(json, JsonSerializerOptions);
    }
}