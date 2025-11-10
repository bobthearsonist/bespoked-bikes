using BespokedBikes.Application.Common;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.SQLite;
using Microsoft.Extensions.DependencyInjection;

namespace BespokedBikes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IDbContextFactory factory,
        Action<IMigrationRunnerBuilder>? configureMigrations = null)
    {
        services.AddSingleton(factory);

        // Register persistent connection if required by the database provider
        if (factory.GetPersistentConnection() is { } persistentConnection) services.AddSingleton(persistentConnection);

        services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(factory.CreateOptions()));
        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        // Add FluentMigrator services if migrations are configured
        if (configureMigrations == null) return services;

        services.AddFluentMigratorCore()
            .ConfigureRunner(configureMigrations)
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
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
        using var provider = CreateTestDataMigrationRunner(GetConnectionString(serviceProvider));
        provider.GetRequiredService<IMigrationRunner>().MigrateUp();
        return serviceProvider;
    }

    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IDbContextFactory>().ConnectionString;
    }

    private static ServiceProvider CreateTestDataMigrationRunner(string connectionString) =>
        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialCreate).Assembly).For.Migrations())
            .Configure<RunnerOptions>(opt => opt.Tags = new[] { "TestData" })
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();

    public static IServiceProvider RollbackTestDataMigrations(this IServiceProvider serviceProvider)
    {
        using var provider = CreateTestDataMigrationRunner(GetConnectionString(serviceProvider));
        provider.GetRequiredService<IMigrationRunner>().MigrateDown(0);
        return serviceProvider;
    }
}
