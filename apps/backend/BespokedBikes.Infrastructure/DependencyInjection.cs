using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
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
        runner.MigrateUp();
        return serviceProvider;
    }
}
