using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Sales;

/// <summary>
/// Service interface for Sales business logic
/// </summary>
public interface ISalesService
{
    Task<Sale> CreateSaleAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime? startDate, DateTime? endDate, SaleStatus? status, CancellationToken cancellationToken = default);
}
