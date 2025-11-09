using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations.TestData;

[Migration(1000004)]
[Tags("TestData")]
public class SeedTestDataInventory : Migration
{
    // Product IDs from 1000001_SeedTestData_Products
    private readonly Guid[] _productIds =
    {
        Guid.Parse("550e8400-e29b-41d4-a716-446655440001"), // Mark III Trail Blazer
        Guid.Parse("550e8400-e29b-41d4-a716-446655440002"), // Lex Racer Elite
        Guid.Parse("550e8400-e29b-41d4-a716-446655440003"), // Model S E-Bike
        Guid.Parse("550e8400-e29b-41d4-a716-446655440004"), // Model 3 E-Bike
        Guid.Parse("550e8400-e29b-41d4-a716-446655440005"), // Model X E-Bike
        Guid.Parse("550e8400-e29b-41d4-a716-446655440006"), // Model Y E-Bike
        Guid.Parse("550e8400-e29b-41d4-a716-446655440007"), // Arc Reactor Gravel
        Guid.Parse("550e8400-e29b-41d4-a716-446655440008"), // Metropolis Commuter
        Guid.Parse("550e8400-e29b-41d4-a716-446655440009")  // Cyberbike FSD
    };

    private readonly Guid[] _inventoryIds =
    {
        Guid.Parse("850e8400-e29b-41d4-a716-446655440001"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440002"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440003"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440004"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440005"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440006"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440007"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440008"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440009"),
        Guid.Parse("850e8400-e29b-41d4-a716-446655440010")
    };

    public override void Up()
    {
        var now = DateTime.UtcNow;

        // Location enum: Store = 0 (only one location)

        // Mark III Trail Blazer inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[0],
                ProductId = _productIds[0],
                Location = 0, // Store
                Quantity = 12,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Lex Racer Elite inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[1],
                ProductId = _productIds[1],
                Location = 0, // Store
                Quantity = 8,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Model S E-Bike inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[2],
                ProductId = _productIds[2],
                Location = 0, // Store
                Quantity = 3,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Model 3 E-Bike inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[3],
                ProductId = _productIds[3],
                Location = 0, // Store
                Quantity = 15,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Model X E-Bike inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[4],
                ProductId = _productIds[4],
                Location = 0, // Store
                Quantity = 5,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Model Y E-Bike inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[5],
                ProductId = _productIds[5],
                Location = 0, // Store
                Quantity = 10,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Arc Reactor Gravel inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[6],
                ProductId = _productIds[6],
                Location = 0, // Store
                Quantity = 7,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Metropolis Commuter inventory
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[7],
                ProductId = _productIds[7],
                Location = 0, // Store
                Quantity = 20,
                CreatedAt = now,
                UpdatedAt = now
            });

        // Cyberbike FSD inventory (coming soon)
        Insert.IntoTable("Inventories")
            .Row(new
            {
                Id = _inventoryIds[8],
                ProductId = _productIds[8],
                Location = 0, // Store
                Quantity = 0,
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    public override void Down()
    {
        foreach (var id in _inventoryIds)
        {
            Delete.FromTable("Inventories")
                .Row(new { Id = id });
        }
    }
}
