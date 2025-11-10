using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Customers;

/// <summary>
/// Service implementation for Customer business logic
/// </summary>
public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(customer, cancellationToken);
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetByIdAsync(id, cancellationToken);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<Customer> UpdateCustomerAsync(Guid id, Customer customer, CancellationToken cancellationToken = default)
    {
        var existingCustomer = await repository.GetByIdAsync(id, cancellationToken);
        if (existingCustomer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        // Update only the editable fields
        existingCustomer.Name = customer.Name;

        return await repository.UpdateAsync(existingCustomer, cancellationToken);
    }
}
