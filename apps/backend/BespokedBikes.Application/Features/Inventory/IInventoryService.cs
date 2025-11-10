using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// Service interface for Inventory business logic
/// </summary>
public interface IInventoryService
{
    Task<Domain.Entities.Inventory> UpdateProductInventoryAsync(Guid productId, Location location, int quantity, CancellationToken cancellationToken = default);
}
