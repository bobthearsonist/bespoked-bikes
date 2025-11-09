using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Products;

public interface IProductService
{
    Task<ProductDto> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ICollection<ProductDto>> ListProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto, CancellationToken cancellationToken = default);
}
