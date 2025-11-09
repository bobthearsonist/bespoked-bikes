using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Customers;

/// <summary>
/// Service implementation for Customer business logic
/// </summary>
public class CustomerService(ICustomerRepository repository, IMapper mapper) : ICustomerService
{
    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken = default)
    {
        var customer = mapper.Map<Customer>(customerDto);
        var createdCustomer = await repository.CreateAsync(customer, cancellationToken);
        return mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetByIdAsync(id, cancellationToken);
        return customer == null ? null : mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(Guid id, CustomerDto customerDto, CancellationToken cancellationToken = default)
    {
        var existingCustomer = await repository.GetByIdAsync(id, cancellationToken);
        if (existingCustomer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        // Update only the editable fields
        existingCustomer.Name = customerDto.Name;

        var updatedCustomer = await repository.UpdateAsync(existingCustomer, cancellationToken);
        return mapper.Map<CustomerDto>(updatedCustomer);
    }
}
