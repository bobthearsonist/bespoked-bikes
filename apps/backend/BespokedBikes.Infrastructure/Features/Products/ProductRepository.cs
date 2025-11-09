using BespokedBikes.Application.Features.Products;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Features.Products;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        product.CreatedAt = now;
        product.UpdatedAt = now;

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.UpdatedAt = DateTime.UtcNow;

        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }
}
