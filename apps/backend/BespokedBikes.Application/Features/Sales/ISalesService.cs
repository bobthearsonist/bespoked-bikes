using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Sales;

/// <summary>
/// Service interface for Sales business logic
/// </summary>
public interface ISalesService
{
    Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto, CancellationToken cancellationToken = default);
    Task<SaleDto?> GetSaleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, Generated.SaleStatus? status, CancellationToken cancellationToken = default);
}
