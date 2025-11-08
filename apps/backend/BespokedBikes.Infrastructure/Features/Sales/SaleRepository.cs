using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Sales;

public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Implementation to be added
}
