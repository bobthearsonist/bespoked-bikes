using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace BespokedBikes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IDbContextFactory factory,
        Action<IMigrationRunnerBuilder>? configureMigrations = null)
    {
        // Register the factory as a singleton
        services.AddSingleton(factory);

        // Add DbContext with options from the factory
        var options = factory.CreateOptions();
        services.AddScoped<ApplicationDbContext>(provider => new ApplicationDbContext(options));

        // Register interface for dependency injection
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Add FluentMigrator services if migrations are configured
        if (configureMigrations != null)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(configureMigrations)
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }

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
