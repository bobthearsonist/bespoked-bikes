using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Products;

public interface IProductService
{
    Task<Product> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListProductsAsync(CancellationToken cancellationToken = default);
    Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> UpdateProductAsync(Guid id, Product product, CancellationToken cancellationToken = default);
}
