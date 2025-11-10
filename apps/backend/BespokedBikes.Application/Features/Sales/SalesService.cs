using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Sales;

/// <summary>
/// Service implementation for Sales business logic
/// </summary>
public class SalesService(
    ISaleRepository saleRepository,
    IProductRepository productRepository,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IInventoryRepository inventoryRepository)
    : ISalesService
{
    public async Task<Sale> CreateSaleAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        // Validate customer exists
        var customer = await customerRepository.GetByIdAsync(sale.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {sale.CustomerId} not found");
        }

        // Validate employee exists
        var employee = await employeeRepository.GetByIdAsync(sale.SoldByEmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {sale.SoldByEmployeeId} not found");
        }

        // Validate product exists and get commission percentage
        var product = await productRepository.GetByIdAsync(sale.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {sale.ProductId} not found");
        }

        // Check if inventory exists for that product at that location
        var inventoryCheck = await inventoryRepository.GetByProductAndLocationAsync(sale.ProductId, sale.Location, cancellationToken);

        // Calculate commission: salePrice * (commissionPercentage / 100)
        sale.CommissionAmount = sale.SalePrice * (product.CommissionPercentage / 100m);

        // Determine sale status based on inventory availability
        sale.Status = inventoryCheck != null && inventoryCheck.Quantity > 0
            ? SaleStatus.Fulfilled
            : SaleStatus.Pending;

        // Save to database
        sale = await saleRepository.CreateAsync(sale, cancellationToken);

        // Update inventory - decrement quantity by 1 if inventory exists
        if (inventoryCheck != null)
        {
            inventoryCheck.Quantity -= 1;
            await inventoryRepository.UpdateAsync(inventoryCheck, cancellationToken);
        }

        return sale;
    }

    public async Task<Sale?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await saleRepository.GetByIdAsync(id, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {id} not found");
        }
        return sale;
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime? startDate, DateTime? endDate, SaleStatus? status, CancellationToken cancellationToken = default)
    {
        // For MVP, filters are ignored and all sales are returned
        // TODO: Implement proper filtering with OData
        return await saleRepository.GetByDateRangeAsync(null, null, null, cancellationToken);
    }
}
