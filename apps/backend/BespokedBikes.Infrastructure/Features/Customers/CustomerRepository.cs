using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Customers;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    // Implementation to be added
}
