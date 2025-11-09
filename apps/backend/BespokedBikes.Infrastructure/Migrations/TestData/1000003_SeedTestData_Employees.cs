using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations.TestData;

[Migration(1000003)]
[Tags("TestData")]
public class SeedTestDataEmployees : Migration
{
    private readonly Guid[] _employeeIds =
    {
        Guid.Parse("750e8400-e29b-41d4-a716-446655440001"),
        Guid.Parse("750e8400-e29b-41d4-a716-446655440002"),
        Guid.Parse("750e8400-e29b-41d4-a716-446655440003"),
        Guid.Parse("750e8400-e29b-41d4-a716-446655440004")
    };

    public override void Up()
    {
        var now = DateTime.UtcNow;

        // Location enum: Store = 0 (only one location)
        // Roles flags: Sales = 1, Warehouse = 2, Admin = 4

        Insert.IntoTable("Employees")
            .Row(new
            {
                Id = _employeeIds[0],
                Name = "Dante Hicks",
                Location = 0, // Store
                Roles = 3, // Sales + Warehouse
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _employeeIds[1],
                Name = "Randal Graves",
                Location = 0, // Store
                Roles = 1, // Sales
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _employeeIds[2],
                Name = "Jay",
                Location = 0, // Store
                Roles = 1, // Sales
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _employeeIds[3],
                Name = "Silent Bob",
                Location = 0, // Store
                Roles = 2, // Warehouse
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    public override void Down()
    {
        foreach (var id in _employeeIds)
        {
            Delete.FromTable("Employees")
                .Row(new { Id = id });
        }
    }
}
