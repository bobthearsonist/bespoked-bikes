using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Customers;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Implementation to be added
}
