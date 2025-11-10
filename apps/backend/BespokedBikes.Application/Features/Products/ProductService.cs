using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Products;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);

        if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public async Task<IReadOnlyList<Product>> ListProductsAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.Id = Guid.NewGuid();
        return await repository.CreateAsync(product, cancellationToken);
    }

    public async Task<Product> UpdateProductAsync(Guid id, Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = await repository.GetByIdAsync(id, cancellationToken);

        if (existingProduct == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

        // Update the existing product's properties while preserving timestamps
        existingProduct.ProductType = product.ProductType;
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Supplier = product.Supplier;
        existingProduct.CostPrice = product.CostPrice;
        existingProduct.RetailPrice = product.RetailPrice;
        existingProduct.CommissionPercentage = product.CommissionPercentage;

        return await repository.UpdateAsync(existingProduct, cancellationToken);
    }
}
