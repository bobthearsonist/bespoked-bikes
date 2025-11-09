using BespokedBikes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Data;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Sale> Sales => Set<Sale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.UpdatedAt).IsRequired();
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.Roles).IsRequired();
            entity.Property(e => e.HireDate).IsRequired();
            entity.Property(e => e.TerminationDate);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.ProductType).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);
            entity.Property(p => p.Supplier).IsRequired().HasMaxLength(200);
            entity.Property(p => p.CostPrice).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.RetailPrice).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.CommissionPercentage).IsRequired().HasPrecision(5, 2);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt).IsRequired();

            entity.HasIndex(p => p.Name);
        });

        // Inventory configuration
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductId).IsRequired();
            entity.Property(i => i.Location).IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.CreatedAt).IsRequired();
            entity.Property(i => i.UpdatedAt).IsRequired();

            // Foreign key to Product
            entity.HasOne<Product>()
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint on ProductId + Location
            entity.HasIndex(i => new { i.ProductId, i.Location }).IsUnique();
        });

        // Sale configuration
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.CustomerId).IsRequired();
            entity.Property(s => s.SoldByEmployeeId).IsRequired();
            entity.Property(s => s.FulfilledByEmployeeId);
            entity.Property(s => s.ProductId).IsRequired();
            entity.Property(s => s.Status).IsRequired();
            entity.Property(s => s.SalePrice).IsRequired().HasPrecision(18, 2);
            entity.Property(s => s.CommissionAmount).IsRequired().HasPrecision(18, 2);
            entity.Property(s => s.SaleChannel).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Location).IsRequired();
            entity.Property(s => s.SaleDate).IsRequired();
            entity.Property(s => s.FulfilledDate);
            entity.Property(s => s.CreatedAt).IsRequired();
            entity.Property(s => s.UpdatedAt).IsRequired();

            // Foreign keys
            entity.HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(s => s.SoldByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(s => s.FulfilledByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(s => s.SaleDate);
            entity.HasIndex(s => s.SoldByEmployeeId);
            entity.HasIndex(s => s.CustomerId);
        });
    }
}
