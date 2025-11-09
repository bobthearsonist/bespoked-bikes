using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Data.Factories;

/// <summary>
/// Factory for creating SQLite in-memory database contexts.
/// This is shared infrastructure for development and can be used by multiple applications
/// (API, batch processors, etc.) that need to share the same in-memory database instance.
/// For production, use a real database factory (e.g., PostgresDbContextFactory).
/// </summary>
public class InMemorySqliteDbContextFactory : IDbContextFactory, IDisposable
{
    private readonly SqliteConnection _connection;

    public string ConnectionString { get; }
    public bool RequiresPersistentConnection => true;

    public InMemorySqliteDbContextFactory()
    {
        ConnectionString = "DataSource=:memory:";
        _connection = new SqliteConnection(ConnectionString);
        _connection.Open(); // Keep connection open for in-memory database lifetime
    }

    public DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(_connection);
        return optionsBuilder.Options;
    }

    public SqliteConnection GetConnection() => _connection;

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
