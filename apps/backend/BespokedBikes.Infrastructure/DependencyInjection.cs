using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BespokedBikes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Create the appropriate database factory based on configuration
        var factory = CreateDbContextFactory(configuration);

        return services.AddInfrastructure(factory, rb => ConfigureMigrations(rb, factory));
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IDbContextFactory factory,
        Action<IMigrationRunnerBuilder>? configureMigrations = null)
    {
        services.AddSingleton(factory);

        // Register persistent connection if required by the database provider
        if (factory.GetPersistentConnection() is { } persistentConnection)
            services.AddSingleton(persistentConnection);

        services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(factory.CreateOptions()));
        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        // Add FluentMigrator services if migrations are configured
        if (configureMigrations != null)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(configureMigrations)
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        return services;
    }

    private static IDbContextFactory CreateDbContextFactory(IConfiguration configuration)
    {
        var databaseProvider = configuration["Database:Provider"] ?? "SQLite";

        return databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase)
            ? CreatePostgreSqlFactory(configuration)
            : CreateSqliteFactory();
    }

    private static IDbContextFactory CreatePostgreSqlFactory(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("PostgreSQL connection string 'DefaultConnection' not found in configuration");

        return new PostgreSqlDbContextFactory(connectionString);
    }

    private static IDbContextFactory CreateSqliteFactory()
    {
        return new InMemorySqliteDbContextFactory();
    }

    private static void ConfigureMigrations(IMigrationRunnerBuilder builder, IDbContextFactory factory)
    {
        var connectionString = factory.ConnectionString;

        if (factory is PostgreSqlDbContextFactory)
        {
            builder.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialCreate).Assembly).For.Migrations();
        }
        else if (factory is InMemorySqliteDbContextFactory)
        {
            builder.AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialCreate).Assembly).For.Migrations();
        }
        else
        {
            throw new InvalidOperationException($"Unsupported database factory type: {factory.GetType().Name}");
        }
    }

    public static IServiceProvider RunMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        // Run all untagged migrations (schema migrations)
        // Tagged migrations (e.g., [Tags("TestData")]) are skipped and run separately
        runner.MigrateUp();
        return serviceProvider;
    }

    public static IServiceProvider RunTestDataMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();
        using var provider = CreateTestDataMigrationRunner(factory);
        provider.GetRequiredService<IMigrationRunner>().MigrateUp();
        return serviceProvider;
    }

    private static ServiceProvider CreateTestDataMigrationRunner(IDbContextFactory factory)
    {
        var services = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                if (factory is PostgreSqlDbContextFactory)
                {
                    rb.AddPostgres();
                }
                else if (factory is InMemorySqliteDbContextFactory)
                {
                    rb.AddSQLite();
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported database factory type: {factory.GetType().Name}");
                }

                rb.WithGlobalConnectionString(factory.ConnectionString)
                  .ScanIn(typeof(InitialCreate).Assembly).For.Migrations();
            })
            .Configure<RunnerOptions>(opt => opt.Tags = new[] { "TestData" })
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services.BuildServiceProvider();
    }

    public static IServiceProvider RollbackTestDataMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();
        using var provider = CreateTestDataMigrationRunner(factory);
        provider.GetRequiredService<IMigrationRunner>().MigrateDown(0);
        return serviceProvider;
    }
}
