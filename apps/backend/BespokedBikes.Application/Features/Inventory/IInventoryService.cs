using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// Service interface for Inventory business logic
/// </summary>
public interface IInventoryService
{
    Task<InventoryDto> UpdateProductInventoryAsync(Guid productId, InventoryUpdateDto updateDto, CancellationToken cancellationToken = default);
}
