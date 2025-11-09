using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Data.Factories;

public interface IDbContextFactory
{
    DbContextOptions<ApplicationDbContext> CreateOptions();

    string ConnectionString { get; }

    bool RequiresPersistentConnection { get; }
}
