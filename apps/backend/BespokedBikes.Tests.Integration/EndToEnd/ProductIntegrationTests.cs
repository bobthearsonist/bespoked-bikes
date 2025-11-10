using FluentAssertions;
using ProductDto = BespokedBikes.Application.Generated.ProductDto;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [TestFixture]
    public class ProductIntegrationTests : IntegrationTestBase
    {
        /// <summary>
        /// Happy path: Create a product and verify it appears in the list
        /// </summary>
        [Test]
        public async Task CreateProduct_ThenList_ShouldReturnProduct()
        {
            // Create a product
            var productDto = new ProductDto
            {
                Name = "Road Bike",
                ProductType = "Bike",
                Description = "High-performance carbon fiber road bike",
                Supplier = "Giant",
                CostPrice = "400.00",
                RetailPrice = "599.99",
                CommissionPercentage = "4.00"
            };
            await Api.CreateProduct(productDto, CancellationToken.None);

            // List products and verify it's there
            var products = await Api.ListProducts(CancellationToken.None);
            products.Should().ContainSingle();
            var product = products.First();
            product.Name.Should().Be("Road Bike");
            product.Supplier.Should().Be("Giant");
        }
    }
}
