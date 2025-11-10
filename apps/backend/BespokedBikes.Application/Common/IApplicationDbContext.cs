using BespokedBikes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Application.Common;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Product> Products { get; }
    DbSet<Inventory> Inventories { get; }
    DbSet<Sale> Sales { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
