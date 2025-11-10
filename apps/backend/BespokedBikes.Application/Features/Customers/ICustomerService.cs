using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Customers;

/// <summary>
/// Service interface for Customer business logic
/// </summary>
public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
    Task<Customer> UpdateCustomerAsync(Guid id, Customer customer, CancellationToken cancellationToken = default);
}
