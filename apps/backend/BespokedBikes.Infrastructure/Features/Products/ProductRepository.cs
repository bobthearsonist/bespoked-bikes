using BespokedBikes.Application.Features.Products;
using BespokedBikes.Infrastructure.Data;

namespace BespokedBikes.Infrastructure.Features.Products;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    // Implementation to be added
}
