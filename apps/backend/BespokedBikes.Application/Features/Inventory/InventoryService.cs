using AutoMapper;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// Service implementation for Inventory business logic
/// </summary>
public class InventoryService(
    IInventoryRepository inventoryRepository,
    IProductRepository productRepository,
    IMapper mapper)
    : IInventoryService
{
    public async Task<InventoryDto> UpdateProductInventoryAsync(Guid productId, InventoryUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        // Verify product exists
        var product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found");
        }

        // Check if inventory record exists for this product/location
        var location = (Domain.Enums.Location)updateDto.Location;
        var existingInventory = await inventoryRepository.GetByProductAndLocationAsync(
            productId,
            location,
            cancellationToken);

        Domain.Entities.Inventory inventory;

        if (existingInventory == null)
        {
            // Create new inventory record
            inventory = new Domain.Entities.Inventory
            {
                ProductId = productId,
                Location = location,
                Quantity = updateDto.Quantity
            };
            inventory = await inventoryRepository.CreateAsync(inventory, cancellationToken);
        }
        else
        {
            // Update existing inventory record
            existingInventory.Quantity = updateDto.Quantity;
            inventory = await inventoryRepository.UpdateAsync(existingInventory, cancellationToken);
        }

        return mapper.Map<InventoryDto>(inventory);
    }
}
