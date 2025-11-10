using System.Data.Common;
using BespokedBikes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Data.Factories;

public class PostgreSqlDbContextFactory : IDbContextFactory
{
    private readonly string _connectionString;

    public PostgreSqlDbContextFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public string ConnectionString => _connectionString;

    public bool RequiresPersistentConnection => false;

    public DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return optionsBuilder.Options;
    }

    public DbConnection? GetPersistentConnection()
    {
        // PostgreSQL doesn't need a persistent connection like in-memory SQLite does
        return null;
    }
}
