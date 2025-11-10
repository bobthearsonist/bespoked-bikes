using AutoMapper;
using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Location = BespokedBikes.Domain.Enums.Location;

namespace BespokedBikes.Application.Features.Sales;

/// <summary>
/// Service implementation for Sales business logic
/// </summary>
public class SalesService(
    ISaleRepository saleRepository,
    IProductRepository productRepository,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IInventoryRepository inventoryRepository,
    IMapper mapper)
    : ISalesService
{
    public async Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto, CancellationToken cancellationToken = default)
    {
        //TODO: Add validation for CreateSaleDto (e.g., required fields, valid values)
        // Validate customer exists
        var customer = await customerRepository.GetByIdAsync(createSaleDto.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {createSaleDto.CustomerId} not found");
        }

        // Validate employee exists
        var employee = await employeeRepository.GetByIdAsync(createSaleDto.SoldByEmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {createSaleDto.SoldByEmployeeId} not found");
        }

        // Validate product exists and get commission percentage
        var product = await productRepository.GetByIdAsync(createSaleDto.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {createSaleDto.ProductId} not found");
        }

        // Check if inventory exists for that product at that location
        var inventoryCheck = await inventoryRepository.GetByProductAndLocationAsync(createSaleDto.ProductId, (Location)createSaleDto.Location, cancellationToken);

        // Parse sale price from string to decimal
        var salePrice = decimal.Parse(createSaleDto.SalePrice, System.Globalization.CultureInfo.InvariantCulture);

        // Calculate commission: salePrice * (commissionPercentage / 100)
        var commissionAmount = salePrice * (product.CommissionPercentage / 100m);

        // Convert DateTimeOffset to DateTime (UTC)
        var saleDate = createSaleDto.SaleDate.UtcDateTime;

        // Determine sale status based on inventory availability
        var saleStatus = inventoryCheck != null && inventoryCheck.Quantity > 0
            ? Domain.Enums.SaleStatus.Fulfilled
            : Domain.Enums.SaleStatus.Pending;

        // Create sale entity
        var sale = new Sale
        {
            CustomerId = createSaleDto.CustomerId,
            SoldByEmployeeId = createSaleDto.SoldByEmployeeId,
            ProductId = createSaleDto.ProductId,
            SalePrice = salePrice,
            CommissionAmount = commissionAmount,
            SaleChannel = createSaleDto.SaleChannel ?? "Unknown",
            Location = (Domain.Enums.Location)createSaleDto.Location,
            SaleDate = saleDate,
            Status = saleStatus,
            FulfilledByEmployeeId = null,
            FulfilledDate = null
        };

        // Save to database
        sale = await saleRepository.CreateAsync(sale, cancellationToken);

        // Update inventory with optimistic concurrency control
        if (inventoryCheck != null)
        {
            const int maxRetries = 3;
            var retryCount = 0;
            var success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    // Reload the inventory to get the latest version
                    if (retryCount > 0)
                    {
                        inventoryCheck = await inventoryRepository.GetByProductAndLocationAsync(
                            sale.ProductId, 
                            sale.Location, 
                            cancellationToken);
                        
                        if (inventoryCheck == null)
                        {
                            // Inventory was deleted, break out
                            break;
                        }
                    }

                    // Check if we have sufficient quantity
                    if (inventoryCheck.Quantity <= 0)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient inventory for product {sale.ProductId} at location {sale.Location}");
                    }

                    // Decrement quantity
                    inventoryCheck.Quantity -= 1;
                    await inventoryRepository.UpdateAsync(inventoryCheck, cancellationToken);
                    success = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw new InvalidOperationException(
                            $"Failed to update inventory after {maxRetries} attempts due to concurrent updates. Please try again.");
                    }
                    // Wait briefly before retrying to reduce contention
                    await Task.Delay(50 * retryCount, cancellationToken);
                }
            }
        }

        // Map to DTO and return
        return mapper.Map<SaleDto>(sale);
    }

    public async Task<SaleDto?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await saleRepository.GetByIdAsync(id, cancellationToken);
        return sale == null ? null : mapper.Map<SaleDto>(sale);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, Generated.SaleStatus? status, CancellationToken cancellationToken = default)
    {
        // For MVP, filters are ignored and all sales are returned
        // TODO: Implement proper filtering with OData
        var sales = await saleRepository.GetByDateRangeAsync(null, null, null, cancellationToken);
        return mapper.Map<IEnumerable<SaleDto>>(sales);
    }
}
