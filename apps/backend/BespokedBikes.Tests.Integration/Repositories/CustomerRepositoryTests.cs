using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Migrations;
using BespokedBikes.Tests.Integration.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BespokedBikes.Tests.Integration.Repositories;

/// <summary>
/// Integration tests using TestcontainerDbContextFactory for PostgreSQL testcontainer.
/// </summary>
[TestFixture]
public class CustomerRepositoryTests
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
        // Create and initialize testcontainer factory
        _factory = new TestcontainerDbContextFactory();
        await _factory.InitializeAsync();

        // Setup services with the factory and configure migrations
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
    public void Setup() =>
        // Create a new scope for each test
        _scope = _serviceProvider!.CreateScope();

    [TearDown]
  public async Task TearDown()
  {
      // Clean up data after each test to ensure isolation
      if (_scope == null) return;

      if (DbContext is not ApplicationDbContext context)
      {
          _scope.Dispose();
          return;
      }

      // Remove all data from tables
      context.Sales.RemoveRange(context.Sales);
      context.Products.RemoveRange(context.Products);
      context.Customers.RemoveRange(context.Customers);
      context.Employees.RemoveRange(context.Employees);
      await context.SaveChangesAsync();

      _scope.Dispose();
  }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_serviceProvider is IDisposable disposable) disposable.Dispose();

        if (_factory != null) await _factory.DisposeAsync();
    }
    [Test]
    public async Task CanCreateAndRetrieveCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act - Save customer
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Assert - Retrieve customer
        var retrievedCustomer = await DbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        Assert.That(retrievedCustomer, Is.Not.Null);
        Assert.That(retrievedCustomer.Name, Is.EqualTo("Test Customer"));
    }

    [Test]
    public async Task CanUpdateCustomer()
    {
        // Arrange - Create customer
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act - Update customer
        var customerToUpdate = await DbContext.Customers
            .FirstAsync(c => c.Id == customer.Id);

        customerToUpdate.Name = "Updated Name";
        customerToUpdate.UpdatedAt = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();

        // Assert - Verify update
        var updatedCustomer = await DbContext.Customers
            .FirstAsync(c => c.Id == customer.Id);

        Assert.That(updatedCustomer.Name, Is.EqualTo("Updated Name"));
    }

    [Test]
    public async Task CanDeleteCustomer()
    {
        // Arrange - Create customer
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Customer To Delete",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        // Act - Delete customer
        var customerToDelete = await DbContext.Customers
            .FirstAsync(c => c.Id == customer.Id);

        DbContext.Customers.Remove(customerToDelete);
        await DbContext.SaveChangesAsync();

        // Assert - Verify deletion
        var deletedCustomer = await DbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        Assert.That(deletedCustomer, Is.Null);
    }

    // TODO this should be in a base test class shared by all repository tests
    [Test]
    public async Task DatabaseIsIsolatedBetweenTests()
    {
        // This test verifies that each test gets a fresh database
        // There should be no customers from previous tests
        var customerCount = await DbContext.Customers.CountAsync();
        Assert.That(customerCount, Is.EqualTo(0));
    }
}
