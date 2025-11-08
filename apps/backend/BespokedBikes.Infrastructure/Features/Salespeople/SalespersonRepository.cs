using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Salespeople;

public class SalespersonRepository : ISalespersonRepository
{
    private readonly ApplicationDbContext _context;

    public SalespersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Implementation to be added
}
