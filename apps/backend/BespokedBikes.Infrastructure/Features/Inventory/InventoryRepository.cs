using BespokedBikes.Application.Common.Interfaces;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Features.Inventory;

/// <summary>
/// Repository implementation for Inventory entity using EF Core
/// </summary>
public class InventoryRepository : IInventoryRepository
{
    private readonly IApplicationDbContext _context;

    public InventoryRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Inventory?> GetByProductAndLocationAsync(Guid productId, Location location, CancellationToken cancellationToken = default)
    {
        return await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.Location == location, cancellationToken);
    }

    public async Task<Domain.Entities.Inventory> CreateAsync(Domain.Entities.Inventory inventory, CancellationToken cancellationToken = default)
    {
        inventory.Id = Guid.NewGuid();
        inventory.CreatedAt = DateTime.UtcNow;
        inventory.UpdatedAt = DateTime.UtcNow;

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync(cancellationToken);

        return inventory;
    }

    public async Task<Domain.Entities.Inventory> UpdateAsync(Domain.Entities.Inventory inventory, CancellationToken cancellationToken = default)
    {
        inventory.UpdatedAt = DateTime.UtcNow;

        _context.Inventories.Update(inventory);
        await _context.SaveChangesAsync(cancellationToken);

        return inventory;
    }

    public async Task<IEnumerable<Domain.Entities.Inventory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.Inventories
            .Where(i => i.ProductId == productId)
            .ToListAsync(cancellationToken);
    }
}
