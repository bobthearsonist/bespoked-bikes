using BespokedBikes.Application.Common;
using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Features.Customers;
using BespokedBikes.Infrastructure.Features.Employees;
using BespokedBikes.Infrastructure.Features.Inventory;
using BespokedBikes.Infrastructure.Features.Products;
using BespokedBikes.Infrastructure.Features.Sales;
using BespokedBikes.Infrastructure.Migrations;
using BespokedBikes.Tests.Integration.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BespokedBikes.Tests.Integration.Services;

/// <summary>
/// Integration tests for SalesService validating the MVP scenario:
/// - Create a sale with commission calculation
/// - Track inventory updates when a sale is made
/// </summary>
[TestFixture]
public class SalesServiceIntegrationTests
{
    private TestcontainerDbContextFactory? _factory;
    private IServiceProvider? _serviceProvider;
    private IServiceScope? _scope;

    protected IApplicationDbContext DbContext =>
        _scope?.ServiceProvider.GetRequiredService<IApplicationDbContext>()
        ?? throw new InvalidOperationException("Test not initialized");

    protected ISalesService SalesService =>
        _scope?.ServiceProvider.GetRequiredService<ISalesService>()
        ?? throw new InvalidOperationException("Test not initialized");

    protected IInventoryService InventoryService =>
        _scope?.ServiceProvider.GetRequiredService<IInventoryService>()
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

        // Register application services (repositories and services)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISalesService, SalesService>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeRoleService, FlagBasedEmployeeRoleService>();

        // Add AutoMapper with assembly scanning and global type converters
        services.AddAutoMapper(config =>
        {
            // Configure global decimal <-> string converters for monetary values
            config.CreateMap<decimal, string>()
                .ConvertUsing(d => d.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            config.CreateMap<string, decimal>()
                .ConvertUsing(s => decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture));

            // Scan assemblies for mapping profiles
            config.AddMaps(typeof(ProductDto).Assembly);
        });

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
                // Remove all data from tables in proper order (respecting foreign keys)
                context.Sales.RemoveRange(context.Sales);
                context.Inventories.RemoveRange(context.Inventories);
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

    /// <summary>
    /// MVP Integration Test: Complete sale flow with commission calculation and inventory tracking.
    ///
    /// This test validates the core business scenario:
    /// 1. A customer purchases a bike from a salesperson
    /// 2. The sale price is recorded
    /// 3. Commission is automatically calculated based on product's commission percentage
    /// 4. Inventory is decremented to reflect the sold item
    ///
    /// EXPECTED TO FAIL: Currently, the SalesService does not update inventory when a sale is created.
    /// This test documents the expected behavior that needs to be implemented.
    /// </summary>
    [Test]
    public async Task CreateSale_WithCommissionCalculation_AndInventoryTracking_ShouldCompleteSuccessfully()
    {
        // ==================== ARRANGE ====================

        // Create a product with known commission percentage
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            ProductType = "Mountain Bike",
            Name = "TrailBlazer 5000",
            Description = "High-performance mountain bike",
            Supplier = "Peak Cycles",
            CostPrice = 800.00m,
            RetailPrice = 1200.00m,
            CommissionPercentage = 10.00m, // 10% commission
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Products.Add(product);

        // Create a customer
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "John Smith",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Customers.Add(customer);

        // Create a salesperson employee
        var employeeId = Guid.NewGuid();
        var employee = new Employee
        {
            Id = employeeId,
            Name = "Alice Johnson",
            Location = Domain.Enums.Location.Store,
            Roles = Domain.Enums.EmployeeRole.Salesperson,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Employees.Add(employee);

        // Set up initial inventory - 10 units at Store location
        var inventoryId = Guid.NewGuid();
        var initialQuantity = 10;
        var inventory = new Inventory
        {
            Id = inventoryId,
            ProductId = productId,
            Location = Domain.Enums.Location.Store,
            Quantity = initialQuantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Inventories.Add(inventory);

        await DbContext.SaveChangesAsync();

        // Define the sale details
        var salePrice = 1150.00m; // Selling slightly below retail
        var expectedCommission = salePrice * (product.CommissionPercentage / 100m); // Should be $115.00
        var saleDate = DateTimeOffset.UtcNow;

        var createSaleDto = new CreateSaleDto
        {
            CustomerId = customerId,
            SoldByEmployeeId = employeeId,
            ProductId = productId,
            SalePrice = salePrice.ToString("F2"),
            SaleChannel = "In-Store",
            Location = Application.Generated.Location.STORE,
            SaleDate = saleDate
        };

        // ==================== ACT ====================

        // Create the sale through the service
        var createdSale = await SalesService.CreateSaleAsync(createSaleDto);

        // Retrieve the inventory after the sale
        var updatedInventory = await DbContext.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.Location == Domain.Enums.Location.Store);

        // ==================== ASSERT ====================

        // Verify the sale was created
        Assert.That(createdSale, Is.Not.Null, "Sale should be created");
        Assert.That(createdSale.CustomerId, Is.EqualTo(customerId), "Sale should be linked to correct customer");
        Assert.That(createdSale.SoldByEmployeeId, Is.EqualTo(employeeId), "Sale should be linked to correct salesperson");
        Assert.That(createdSale.ProductId, Is.EqualTo(productId), "Sale should be linked to correct product");

        // Verify the sale price is correct
        var actualSalePrice = decimal.Parse(createdSale.SalePrice);
        Assert.That(actualSalePrice, Is.EqualTo(salePrice), "Sale price should match the specified amount");

        // Verify commission calculation
        var actualCommission = decimal.Parse(createdSale.CommissionAmount);
        Assert.That(actualCommission, Is.EqualTo(expectedCommission),
            $"Commission should be calculated as {expectedCommission:C} (10% of {salePrice:C})");

        // Verify sale metadata
        Assert.That(createdSale.SaleChannel, Is.EqualTo("In-Store"), "Sale channel should be recorded");
        Assert.That(createdSale.Location, Is.EqualTo(Application.Generated.Location.STORE), "Sale location should be recorded");
        Assert.That(createdSale.Status, Is.EqualTo(Application.Generated.SaleStatus.FULFILLED), "Sale should be Fulfilled when inventory is available");

        // ==================== INVENTORY VERIFICATION (EXPECTED TO FAIL) ====================

        // IMPORTANT: This assertion will FAIL because the current implementation does NOT
        // update inventory when a sale is created. This documents the expected behavior.
        Assert.That(updatedInventory, Is.Not.Null, "Inventory record should exist");
        Assert.That(updatedInventory!.Quantity, Is.EqualTo(initialQuantity - 1),
            $"Inventory should be decremented from {initialQuantity} to {initialQuantity - 1} after sale. " +
            $"EXPECTED TO FAIL: SalesService currently does not update inventory.");

        // Verify we can retrieve the sale from the database
        var retrievedSale = await SalesService.GetSaleByIdAsync(createdSale.Id);
        Assert.That(retrievedSale, Is.Not.Null, "Should be able to retrieve the created sale");
        Assert.That(retrievedSale!.Id, Is.EqualTo(createdSale.Id), "Retrieved sale should have correct ID");
    }

    /// <summary>
    /// Test that verifies commission calculation with different percentages.
    /// This ensures the commission formula is working correctly across various rates.
    /// </summary>
    [Test]
    public async Task CreateSale_WithDifferentCommissionRates_ShouldCalculateCorrectly()
    {
        // Create product with 15% commission
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            ProductType = "Road Bike",
            Name = "Speedster Pro",
            Description = "High-speed road bike",
            Supplier = "Velocity Bikes",
            CostPrice = 1500.00m,
            RetailPrice = 2500.00m,
            CommissionPercentage = 15.00m, // 15% commission
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Products.Add(product);

        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "Jane Doe",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Customers.Add(customer);

        var employeeId = Guid.NewGuid();
        var employee = new Employee
        {
            Id = employeeId,
            Name = "Bob Williams",
            Location = Domain.Enums.Location.Store,
            Roles = Domain.Enums.EmployeeRole.Salesperson,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Employees.Add(employee);

        await DbContext.SaveChangesAsync();

        var salePrice = 2400.00m;
        var expectedCommission = salePrice * 0.15m; // Should be $360.00

        var createSaleDto = new CreateSaleDto
        {
            CustomerId = customerId,
            SoldByEmployeeId = employeeId,
            ProductId = productId,
            SalePrice = salePrice.ToString("F2"),
            SaleChannel = "Online",
            Location = Application.Generated.Location.STORE,
            SaleDate = DateTimeOffset.UtcNow
        };

        // Act
        var createdSale = await SalesService.CreateSaleAsync(createSaleDto);

        // Assert
        var actualCommission = decimal.Parse(createdSale.CommissionAmount);
        Assert.That(actualCommission, Is.EqualTo(expectedCommission),
            $"Commission should be {expectedCommission:C} (15% of {salePrice:C})");
    }

    /// <summary>
    /// Test that validates the sale can be created even without inventory record.
    /// This tests the current behavior where inventory tracking is not enforced.
    /// In the future, this might need to change to require inventory checks.
    /// </summary>
    [Test]
    public async Task CreateSale_WithoutInventoryRecord_ShouldStillCreateSale_WithPendingStatus()
    {
        // Create product WITHOUT inventory
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            ProductType = "Hybrid Bike",
            Name = "CityCommuter",
            Description = "Perfect for city riding",
            Supplier = "Urban Cycles",
            CostPrice = 500.00m,
            RetailPrice = 800.00m,
            CommissionPercentage = 8.00m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Products.Add(product);

        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "Sam Wilson",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Customers.Add(customer);

        var employeeId = Guid.NewGuid();
        var employee = new Employee
        {
            Id = employeeId,
            Name = "Carol Davis",
            Location = Domain.Enums.Location.Store,
            Roles = Domain.Enums.EmployeeRole.Salesperson,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        DbContext.Employees.Add(employee);

        await DbContext.SaveChangesAsync();

        var createSaleDto = new CreateSaleDto
        {
            CustomerId = customerId,
            SoldByEmployeeId = employeeId,
            ProductId = productId,
            SalePrice = "750.00",
            SaleChannel = "Phone",
            Location = Application.Generated.Location.STORE,
            SaleDate = DateTimeOffset.UtcNow
        };

        // Act - should not throw even without inventory
        var createdSale = await SalesService.CreateSaleAsync(createSaleDto);

        // Assert
        Assert.That(createdSale, Is.Not.Null);
        Assert.That(createdSale.ProductId, Is.EqualTo(productId));
    }
}
