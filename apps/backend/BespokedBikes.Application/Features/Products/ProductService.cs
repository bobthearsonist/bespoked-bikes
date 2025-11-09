using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Products;

public class ProductService(IProductRepository repository, IMapper mapper) : IProductService
{
    public async Task<ProductDto> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);

        if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

        return mapper.Map<ProductDto>(product);
    }

    public async Task<ICollection<ProductDto>> ListProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<ICollection<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = mapper.Map<Product>(productDto);
        product.Id = Guid.NewGuid();

        var createdProduct = await repository.CreateAsync(product, cancellationToken);
        return mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var existingProduct = await repository.GetByIdAsync(id, cancellationToken);

        if (existingProduct == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

        // Map the DTO to the existing entity to preserve CreatedAt
        mapper.Map(productDto, existingProduct);
        existingProduct.Id = id;

        var updatedProduct = await repository.UpdateAsync(existingProduct, cancellationToken);
        return mapper.Map<ProductDto>(updatedProduct);
    }
}
