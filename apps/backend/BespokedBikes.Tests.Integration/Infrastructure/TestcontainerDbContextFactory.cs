using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BespokedBikes.Tests.Integration.Infrastructure;

/// <summary>
/// Factory for integration tests that manages its own testcontainer lifecycle.
/// Each test fixture creates its own factory instance which starts a single container
/// shared across all tests in that fixture. The container is disposed when the fixture completes.
/// Tests must handle their own data cleanup between test methods to ensure isolation.
/// </summary>
public class TestcontainerDbContextFactory : IDbContextFactory, IAsyncDisposable
{
    private readonly PostgreSqlContainer _container;
    private bool _isInitialized;

    public string ConnectionString { get; private set; } = string.Empty;
    public bool RequiresPersistentConnection => false;

    public TestcontainerDbContextFactory()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("bespoked_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        await _container.StartAsync();
        ConnectionString = _container.GetConnectionString();
        _isInitialized = true;
    }

    public DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException(
                "Factory must be initialized with InitializeAsync before creating options");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);
        return optionsBuilder.Options;
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
