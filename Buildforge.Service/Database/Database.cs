using Npgsql;

namespace Buildforge.Service.Database;

public sealed class Database : IAsyncDisposable
{
    public NpgsqlDataSource DataSource { get; }

    public Database(IOptions<DatabaseOptions> databaseOption)
    {
        DataSource = NpgsqlDataSource.Create(databaseOption.Value.ToConnectionString());
    }

    public async ValueTask DisposeAsync()
    {
        await DataSource.DisposeAsync();
    }
}