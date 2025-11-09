using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// Repository interface for Inventory entity operations
/// </summary>
public interface IInventoryRepository
{
    Task<Domain.Entities.Inventory?> GetByProductAndLocationAsync(Guid productId, Location location, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Inventory> CreateAsync(Domain.Entities.Inventory inventory, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Inventory> UpdateAsync(Domain.Entities.Inventory inventory, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Inventory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
