using BespokedBikes.Application.Common;
using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Features.Sales;

/// <summary>
/// Repository implementation for Sale entity using EF Core
/// </summary>
public class SaleRepository(IApplicationDbContext context) : ISaleRepository
{
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.Id = Guid.NewGuid();
        sale.CreatedAt = DateTime.UtcNow;
        sale.UpdatedAt = DateTime.UtcNow;

        context.Sales.Add(sale);
        await context.SaveChangesAsync(cancellationToken);

        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Sales
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime? startDate, DateTime? endDate, SaleStatus? status, CancellationToken cancellationToken = default)
    {
        // For MVP, ignore filters and return all sales
        // TODO: Implement proper filtering with OData
        return await context.Sales
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.UpdatedAt = DateTime.UtcNow;

        context.Sales.Update(sale);
        await context.SaveChangesAsync(cancellationToken);

        return sale;
    }
}
