using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations.TestData;

[Migration(1000001)]
[Tags("TestData")]
public class SeedTestDataProducts : Migration
{
    private readonly Guid[] _productIds =
    {
        Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440005"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440006"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440007"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440008"),
        Guid.Parse("550e8400-e29b-41d4-a716-446655440009")
    };

    public override void Up()
    {
        var now = DateTime.UtcNow;

        Insert.IntoTable("Products")
            .Row(new
            {
                Id = _productIds[0],
                ProductType = "Mountain Bike",
                Name = "Mark III Trail Blazer",
                Description = "Hardtail mountain bike with 29-inch wheels, perfect for trail riding",
                Supplier = "Stark Industries",
                CostPrice = 800.00m,
                RetailPrice = 1299.99m,
                CommissionPercentage = 8.5m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[1],
                ProductType = "Road Bike",
                Name = "Lex Racer Elite",
                Description = "Lightweight aluminum road bike for speed and endurance",
                Supplier = "LexCorp",
                CostPrice = 950.00m,
                RetailPrice = 1599.99m,
                CommissionPercentage = 10.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[2],
                ProductType = "Electric Bike",
                Name = "Model S E-Bike",
                Description = "Premium electric bike with long-range battery",
                Supplier = "Tesla",
                CostPrice = 2200.00m,
                RetailPrice = 3499.99m,
                CommissionPercentage = 12.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[3],
                ProductType = "Electric Bike",
                Name = "Model 3 E-Bike",
                Description = "Affordable electric bike for everyday commuting",
                Supplier = "Tesla",
                CostPrice = 1200.00m,
                RetailPrice = 1999.99m,
                CommissionPercentage = 10.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[4],
                ProductType = "Electric Bike",
                Name = "Model X E-Bike",
                Description = "High-performance electric bike with dual motors",
                Supplier = "Tesla",
                CostPrice = 2800.00m,
                RetailPrice = 4499.99m,
                CommissionPercentage = 13.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[5],
                ProductType = "Electric Bike",
                Name = "Model Y E-Bike",
                Description = "Versatile electric bike for urban and trail riding",
                Supplier = "Tesla",
                CostPrice = 1800.00m,
                RetailPrice = 2999.99m,
                CommissionPercentage = 11.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[6],
                ProductType = "Gravel Bike",
                Name = "Arc Reactor Gravel",
                Description = "Versatile gravel bike for mixed terrain adventures",
                Supplier = "Stark Industries",
                CostPrice = 1200.00m,
                RetailPrice = 2099.99m,
                CommissionPercentage = 9.5m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[7],
                ProductType = "Hybrid Bike",
                Name = "Metropolis Commuter",
                Description = "Comfortable hybrid bike for casual riding and commuting",
                Supplier = "LexCorp",
                CostPrice = 250.00m,
                RetailPrice = 449.99m,
                CommissionPercentage = 7.0m,
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _productIds[8],
                ProductType = "Electric Bike",
                Name = "Cyberbike FSD",
                Description = "Revolutionary electric bike with Full Self-Driving capability",
                Supplier = "Tesla",
                CostPrice = 5000.00m,
                RetailPrice = 9999.99m,
                CommissionPercentage = 15.0m,
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    public override void Down()
    {
        foreach (var id in _productIds)
        {
            Delete.FromTable("Products")
                .Row(new { Id = id });
        }
    }
}
