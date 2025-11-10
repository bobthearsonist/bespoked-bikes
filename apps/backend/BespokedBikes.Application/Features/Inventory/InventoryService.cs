using BespokedBikes.Application.Features.Products;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// Service implementation for Inventory business logic
/// </summary>
public class InventoryService(
    IInventoryRepository inventoryRepository,
    IProductRepository productRepository)
    : IInventoryService
{
    public async Task<Domain.Entities.Inventory> UpdateProductInventoryAsync(Guid productId, Location location, int quantity, CancellationToken cancellationToken = default)
    {
        // Verify product exists
        var product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found");
        }

        // Check if inventory record exists for this product/location
        var existingInventory = await inventoryRepository.GetByProductAndLocationAsync(
            productId,
            location,
            cancellationToken);

        if (existingInventory == null)
        {
            // Create new inventory record
            var inventory = new Domain.Entities.Inventory
            {
                ProductId = productId,
                Location = location,
                Quantity = quantity
            };
            return await inventoryRepository.CreateAsync(inventory, cancellationToken);
        }
        else
        {
            // Update existing inventory record
            existingInventory.Quantity = quantity;
            return await inventoryRepository.UpdateAsync(existingInventory, cancellationToken);
        }
    }
}
