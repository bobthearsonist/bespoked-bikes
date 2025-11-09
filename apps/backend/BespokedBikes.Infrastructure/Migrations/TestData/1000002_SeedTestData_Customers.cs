using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations.TestData;

[Migration(1000002)]
[Tags("TestData")]
public class SeedTestDataCustomers : Migration
{
    private readonly Guid[] _customerIds =
    {
        Guid.Parse("650e8400-e29b-41d4-a716-446655440001"),
        Guid.Parse("650e8400-e29b-41d4-a716-446655440002"),
        Guid.Parse("650e8400-e29b-41d4-a716-446655440003"),
        Guid.Parse("650e8400-e29b-41d4-a716-446655440004"),
        Guid.Parse("650e8400-e29b-41d4-a716-446655440005"),
        Guid.Parse("650e8400-e29b-41d4-a716-446655440006")
    };

    public override void Up()
    {
        var now = DateTime.UtcNow;

        Insert.IntoTable("Customers")
            .Row(new
            {
                Id = _customerIds[0],
                Name = "Veronica Loughran",
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _customerIds[1],
                Name = "Caitlin Bree",
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _customerIds[2],
                Name = "Carol Davis",
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _customerIds[3],
                Name = "David Martinez",
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _customerIds[4],
                Name = "Emma Wilson",
                CreatedAt = now,
                UpdatedAt = now
            })
            .Row(new
            {
                Id = _customerIds[5],
                Name = "Frank Brown",
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    public override void Down()
    {
        foreach (var id in _customerIds)
        {
            Delete.FromTable("Customers")
                .Row(new { Id = id });
        }
    }
}
