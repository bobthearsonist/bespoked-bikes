using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Migrations;
using BespokedBikes.Tests.Integration.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace BespokedBikes.Tests.Integration.Repositories;

/// <summary>
/// Example showing how to use a custom factory created in the test project.
/// This gives you maximum flexibility to customize the database setup for your specific test needs.
/// </summary>
[TestFixture]
public class ProductRepositoryTests
{
    private TestcontainerDbContextFactory? _factory;
    private IServiceProvider? _serviceProvider;
    private IServiceScope? _scope;

    protected IApplicationDbContext DbContext =>
        _scope?.ServiceProvider.GetRequiredService<IApplicationDbContext>()
        ?? throw new InvalidOperationException("Test not initialized");

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        // Create and initialize our custom factory
        _factory = new TestcontainerDbContextFactory();
        await _factory.InitializeAsync();

        // Setup services with the custom factory and configure migrations
        var services = new ServiceCollection();
        services.AddInfrastructure(
            _factory,
            rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(_factory.ConnectionString)
                .ScanIn(typeof(InitialCreate).Assembly).For.Migrations());

        _serviceProvider = services.BuildServiceProvider();
        _serviceProvider.RunMigrations();
    }

    [SetUp]
    public void Setup()
    {
        // Create a new scope for each test
        _scope = _serviceProvider!.CreateScope();
    }

    [TearDown]
    public async Task TearDown()
    {
        // Clean up data after each test to ensure isolation
        if (_scope != null)
        {
            var context = DbContext as ApplicationDbContext;
            if (context != null)
            {
                // Remove all data from tables (in correct order to respect foreign keys)
                context.Sales.RemoveRange(context.Sales);
                context.Products.RemoveRange(context.Products);
                context.Customers.RemoveRange(context.Customers);
                context.Employees.RemoveRange(context.Employees);
                await context.SaveChangesAsync();
            }

            _scope.Dispose();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }

    [Test]
    public async Task CanCreateProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            ProductType = "Bike",
            Name = "Mountain Bike Pro",
            Description = "High-performance mountain bike",
            Supplier = "BikeSupplier Inc",
            CostPrice = 500.00m,
            RetailPrice = 1000.00m,
            CommissionPercentage = 15.00m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        DbContext.Products.Add(product);
        await DbContext.SaveChangesAsync();

        // Assert
        var savedProduct = await DbContext.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        Assert.That(savedProduct, Is.Not.Null);
        Assert.That(savedProduct.Name, Is.EqualTo("Mountain Bike Pro"));
        Assert.That(savedProduct.RetailPrice, Is.EqualTo(1000.00m));
    }

    [Test]
    public async Task CanQueryProductsByName()
    {
        // Arrange - Create multiple products
        var products = new[]
        {
            new Product
            {
                Id = Guid.NewGuid(),
                ProductType = "Bike",
                Name = "Road Bike",
                Description = "Fast road bike",
                Supplier = "BikeSupplier Inc",
                CostPrice = 600.00m,
                RetailPrice = 1200.00m,
                CommissionPercentage = 12.00m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                ProductType = "Bike",
                Name = "Mountain Bike",
                Description = "Sturdy mountain bike",
                Supplier = "BikeSupplier Inc",
                CostPrice = 700.00m,
                RetailPrice = 1400.00m,
                CommissionPercentage = 12.00m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        DbContext.Products.AddRange(products);
        await DbContext.SaveChangesAsync();

        // Act - Query by name pattern
        var mountainBikes = await DbContext.Products
            .Where(p => p.Name.Contains("Mountain"))
            .ToListAsync();

        // Assert
        Assert.That(mountainBikes, Has.Count.EqualTo(1));
        Assert.That(mountainBikes[0].Name, Is.EqualTo("Mountain Bike"));
    }
}
