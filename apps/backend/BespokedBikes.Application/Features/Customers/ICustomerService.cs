using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Customers;

/// <summary>
/// Service interface for Customer business logic
/// </summary>
public interface ICustomerService
{
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
    Task<CustomerDto> UpdateCustomerAsync(Guid id, CustomerDto customerDto, CancellationToken cancellationToken = default);
}
