using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Sales;

public class SaleRepository(ApplicationDbContext context) : ISaleRepository
{
    private readonly ApplicationDbContext _context = context;

    // Implementation to be added
}
