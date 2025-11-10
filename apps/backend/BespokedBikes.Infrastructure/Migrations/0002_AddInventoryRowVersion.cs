using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations;

[Migration(2)]
public class AddInventoryRowVersion : Migration
{
    public override void Up()
    {
        // Add RowVersion column for optimistic concurrency control
        // Using BYTEA for PostgreSQL, which is compatible with EF Core's RowVersion
        Alter.Table("Inventories")
            .AddColumn("RowVersion").AsBinary().NotNullable().WithDefaultValue(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
    }

    public override void Down()
    {
        Delete.Column("RowVersion").FromTable("Inventories");
    }
}
