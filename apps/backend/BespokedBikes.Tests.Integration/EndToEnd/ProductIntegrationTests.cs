using FluentAssertions;
using BespokedBikes.Tests.Integration.Generated;
using Refit;
using System.Net;
using ProductDto = BespokedBikes.Application.Generated.ProductDto;
using InventoryDto = BespokedBikes.Application.Generated.InventoryDto;
using InventoryUpdateDto = BespokedBikes.Application.Generated.InventoryUpdateDto;
using Location = BespokedBikes.Application.Generated.Location;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [TestFixture]
    public class ProductIntegrationTests : IntegrationTestBase
    {
        /// <summary>
        /// Happy path: Create a product and verify it appears in the list
        ///
        /// TODO: Failing with 500 - AutoMapper needs decimal<->string converters for price fields
        /// </summary>
        [Test]
        public async Task CreateProduct_ThenList_ShouldReturnProduct()
        {
            // Create a product
            var productDto = new ProductDto
            {
                Name = "Road Bike",
                ProductType = "Bike",
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
