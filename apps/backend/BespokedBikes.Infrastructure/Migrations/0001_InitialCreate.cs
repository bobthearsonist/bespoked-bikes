using System.Data;
using FluentMigrator;

namespace BespokedBikes.Infrastructure.Migrations;

[Migration(1)]
public class InitialCreate : Migration
{
    public override void Up()
    {
        // Create Customers table
        Create.Table("Customers")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

        // Create Employees table
        Create.Table("Employees")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Location").AsInt32().NotNullable()
            .WithColumn("Roles").AsInt32().NotNullable()
            .WithColumn("HireDate").AsDateTime().NotNullable()
            .WithColumn("TerminationDate").AsDateTime().Nullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

        // Create Products table
        Create.Table("Products")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ProductType").AsString(200).NotNullable()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Description").AsString(1000).NotNullable()
            .WithColumn("Supplier").AsString(200).NotNullable()
            .WithColumn("CostPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("RetailPrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("CommissionPercentage").AsDecimal(5, 2).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

        Create.Index("IX_Products_Name")
            .OnTable("Products")
            .OnColumn("Name");

        // Create Inventories table
        Create.Table("Inventories")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ProductId").AsGuid().NotNullable()
            .WithColumn("Location").AsInt32().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

        Create.ForeignKey("FK_Inventories_Products")
            .FromTable("Inventories").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id")
            .OnDelete(Rule.None);

        Create.Index("IX_Inventories_ProductId_Location")
            .OnTable("Inventories")
            .OnColumn("ProductId").Ascending()
            .OnColumn("Location").Ascending()
            .WithOptions().Unique();

        // Create Sales table
        Create.Table("Sales")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CustomerId").AsGuid().NotNullable()
            .WithColumn("SoldByEmployeeId").AsGuid().NotNullable()
            .WithColumn("FulfilledByEmployeeId").AsGuid().Nullable()
            .WithColumn("ProductId").AsGuid().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("SalePrice").AsDecimal(18, 2).NotNullable()
            .WithColumn("CommissionAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("SaleChannel").AsString(200).NotNullable()
            .WithColumn("Location").AsInt32().NotNullable()
            .WithColumn("SaleDate").AsDateTime().NotNullable()
            .WithColumn("FulfilledDate").AsDateTime().Nullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

        Create.ForeignKey("FK_Sales_Products")
            .FromTable("Sales").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id")
            .OnDelete(Rule.None);

        Create.ForeignKey("FK_Sales_SoldByEmployee")
            .FromTable("Sales").ForeignColumn("SoldByEmployeeId")
            .ToTable("Employees").PrimaryColumn("Id")
            .OnDelete(Rule.None);

        Create.ForeignKey("FK_Sales_FulfilledByEmployee")
            .FromTable("Sales").ForeignColumn("FulfilledByEmployeeId")
            .ToTable("Employees").PrimaryColumn("Id")
            .OnDelete(Rule.None);

        Create.ForeignKey("FK_Sales_Customers")
            .FromTable("Sales").ForeignColumn("CustomerId")
            .ToTable("Customers").PrimaryColumn("Id")
            .OnDelete(Rule.None);

        Create.Index("IX_Sales_SaleDate")
            .OnTable("Sales")
            .OnColumn("SaleDate");

        Create.Index("IX_Sales_SoldByEmployeeId")
            .OnTable("Sales")
            .OnColumn("SoldByEmployeeId");

        Create.Index("IX_Sales_CustomerId")
            .OnTable("Sales")
            .OnColumn("CustomerId");
    }

    public override void Down()
    {
        Delete.Table("Sales");
        Delete.Table("Inventories");
        Delete.Table("Products");
        Delete.Table("Employees");
        Delete.Table("Customers");
    }
}
