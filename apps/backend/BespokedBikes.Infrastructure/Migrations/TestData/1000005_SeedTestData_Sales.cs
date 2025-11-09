using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations.TestData;

[Migration(1000005)]
[Tags("TestData")]
public class SeedTestDataSales : Migration
{
    // Reference IDs from previous seed migrations
    private readonly Guid[] _customerIds =
    {
        Guid.Parse("650e8400-e29b-41d4-a716-446655440001"), // Veronica
        Guid.Parse("650e8400-e29b-41d4-a716-446655440002"), // Caitlin
        Guid.Parse("650e8400-e29b-41d4-a716-446655440003"), // Willam Black
        Guid.Parse("650e8400-e29b-41d4-a716-446655440004")  // Sanford
    };

    private readonly Guid[] _employeeIds =
    {
        Guid.Parse("750e8400-e29b-41d4-a716-446655440001"), // Dante
        Guid.Parse("750e8400-e29b-41d4-a716-446655440002"), // Randal
        Guid.Parse("750e8400-e29b-41d4-a716-446655440003"), // Jay
        Guid.Parse("750e8400-e29b-41d4-a716-446655440004")  // Silent Bob
    };

    private readonly Guid[] _productIds =
    {
        Guid.Parse("550e8400-e29b-41d4-a716-446655440001"), // Trek X-Caliber 8
        Guid.Parse("550e8400-e29b-41d4-a716-446655440002"), // Specialized Allez Elite
        Guid.Parse("550e8400-e29b-41d4-a716-446655440003"), // Cannondale Quick Neo SL 2
        Guid.Parse("550e8400-e29b-41d4-a716-446655440004")  // Giant Revolt Advanced 2
    };

    private readonly Guid[] _saleIds =
    {
        Guid.Parse("950e8400-e29b-41d4-a716-446655440001"),
        Guid.Parse("950e8400-e29b-41d4-a716-446655440002"),
        Guid.Parse("950e8400-e29b-41d4-a716-446655440003"),
        Guid.Parse("950e8400-e29b-41d4-a716-446655440004"),
        Guid.Parse("950e8400-e29b-41d4-a716-446655440005"),
        Guid.Parse("950e8400-e29b-41d4-a716-446655440006")
    };

    public override void Up()
    {
        var now = DateTime.UtcNow;
        var oneWeekAgo = now.AddDays(-7);
        var twoWeeksAgo = now.AddDays(-14);
        var threeWeeksAgo = now.AddDays(-21);

        // Location enum: Store = 0 (only one location)
        // SaleStatus enum: Pending = 0, Fulfilled = 1, Cancelled = 2

        // Dante sells to Veronica
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[0],
                CustomerId = _customerIds[0], // Veronica
                SoldByEmployeeId = _employeeIds[0], // Dante
                FulfilledByEmployeeId = _employeeIds[0], // Dante
                ProductId = _productIds[0],
                Status = 1, // Fulfilled
                SalePrice = 1299.99m,
                CommissionAmount = 110.50m, // 8.5% commission
                SaleChannel = "In-Store",
                Location = 0, // Store
                SaleDate = threeWeeksAgo,
                FulfilledDate = threeWeeksAgo.AddDays(1),
                CreatedAt = threeWeeksAgo,
                UpdatedAt = threeWeeksAgo.AddDays(1)
            });

        // Dante sells to Caitlin
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[1],
                CustomerId = _customerIds[1], // Caitlin
                SoldByEmployeeId = _employeeIds[0], // Dante
                FulfilledByEmployeeId = _employeeIds[0], // Dante
                ProductId = _productIds[1],
                Status = 1, // Fulfilled
                SalePrice = 1599.99m,
                CommissionAmount = 160.00m, // 10% commission
                SaleChannel = "Online",
                Location = 0, // Store
                SaleDate = twoWeeksAgo,
                FulfilledDate = twoWeeksAgo.AddDays(2),
                CreatedAt = twoWeeksAgo,
                UpdatedAt = twoWeeksAgo.AddDays(2)
            });

        // Randal makes a sale, Silent Bob fulfills
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[2],
                CustomerId = _customerIds[2], // Willam Black
                SoldByEmployeeId = _employeeIds[1], // Randal
                FulfilledByEmployeeId = _employeeIds[3], // Silent Bob
                ProductId = _productIds[2],
                Status = 1, // Fulfilled
                SalePrice = 2999.99m,
                CommissionAmount = 360.00m, // 12% commission
                SaleChannel = "In-Store",
                Location = 0, // Store
                SaleDate = oneWeekAgo,
                FulfilledDate = oneWeekAgo.AddDays(1),
                CreatedAt = oneWeekAgo,
                UpdatedAt = oneWeekAgo.AddDays(1)
            });

        // Dante's pending sale
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[3],
                CustomerId = _customerIds[3], // Sanford
                SoldByEmployeeId = _employeeIds[0], // Dante
                FulfilledByEmployeeId = (Guid?)null,
                ProductId = _productIds[3],
                Status = 0, // Pending
                SalePrice = 2099.99m,
                CommissionAmount = 199.50m, // 9.5% commission
                SaleChannel = "Online",
                Location = 0, // Store
                SaleDate = now.AddDays(-2),
                FulfilledDate = (DateTime?)null,
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-2)
            });

        // Jay makes a sale, Silent Bob fulfills
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[4],
                CustomerId = _customerIds[0], // Veronica
                SoldByEmployeeId = _employeeIds[2], // Jay
                FulfilledByEmployeeId = _employeeIds[3], // Silent Bob
                ProductId = _productIds[0],
                Status = 1, // Fulfilled
                SalePrice = 1299.99m,
                CommissionAmount = 110.50m, // 8.5% commission
                SaleChannel = "In-Store",
                Location = 0, // Store
                SaleDate = oneWeekAgo.AddDays(-3),
                FulfilledDate = oneWeekAgo.AddDays(-2),
                CreatedAt = oneWeekAgo.AddDays(-3),
                UpdatedAt = oneWeekAgo.AddDays(-2)
            });

        // Randal sells to Caitlin, Silent Bob fulfills
        Insert.IntoTable("Sales")
            .Row(new
            {
                Id = _saleIds[5],
                CustomerId = _customerIds[1], // Caitlin
                SoldByEmployeeId = _employeeIds[1], // Randal
                FulfilledByEmployeeId = _employeeIds[3], // Silent Bob
                ProductId = _productIds[1],
                Status = 1, // Fulfilled
                SalePrice = 1599.99m,
                CommissionAmount = 160.00m, // 10% commission
                SaleChannel = "Phone",
                Location = 0, // Store
                SaleDate = now.AddDays(-1),
                FulfilledDate = now,
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now
            });
    }

    public override void Down()
    {
        foreach (var id in _saleIds)
        {
            Delete.FromTable("Sales")
                .Row(new { Id = id });
        }
    }
}
