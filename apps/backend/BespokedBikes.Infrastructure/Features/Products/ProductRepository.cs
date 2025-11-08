using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Products;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Implementation to be added
}
