using FluentAssertions;
using CustomerDto = BespokedBikes.Application.Generated.CustomerDto;
using EmployeeDto = BespokedBikes.Application.Generated.EmployeeDto;
using ProductDto = BespokedBikes.Application.Generated.ProductDto;
using CreateSaleDto = BespokedBikes.Application.Generated.CreateSaleDto;
using InventoryUpdateDto = BespokedBikes.Application.Generated.InventoryUpdateDto;
using Location = BespokedBikes.Application.Generated.Location;
using EmployeeRole = BespokedBikes.Application.Generated.EmployeeRole;
using SaleStatus = BespokedBikes.Application.Generated.SaleStatus;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [TestFixture]
    public class SalesIntegrationTests : IntegrationTestBase
    {
        /// <summary>
        /// Happy path integration test: Create dependencies → Create Sale → Read Sale → Query Sales
        /// Tests the complete sales flow through the HTTP API
        /// </summary>
        [Test]
        public async Task SaleCreation_HappyPath()
        {
            // Set up required dependencies
            var customer = await Api.CreateCustomer(new CustomerDto { Name = "Test Customer" }, CancellationToken.None);
            var employee = await Api.CreateEmployee(new EmployeeDto
            {
                Name = "Test Salesperson",
                Location = Location.STORE,
                Roles = new List<EmployeeRole> { EmployeeRole.SALESPERSON }
            }, CancellationToken.None);
            var product = await Api.CreateProduct(new ProductDto
            {
                Name = "Test Bike",
                ProductType = "Bike",
                Description = "Test bike for integration testing",
                Supplier = "Test Supplier",
                CostPrice = "400.00",
                RetailPrice = "599.99",
                CommissionPercentage = "5.00"
            }, CancellationToken.None);

            // Update inventory to have stock
            await Api.UpdateProductInventory(product.Id, new InventoryUpdateDto
            {
                Location = Location.STORE,
                Quantity = 10
            }, CancellationToken.None);

            // Create a sale
            var saleDto = new CreateSaleDto
            {
                CustomerId = customer.Id,
                SoldByEmployeeId = employee.Id,
                ProductId = product.Id,
                SaleDate = DateTimeOffset.Now,
                SalePrice = "550.00",
                Location = Location.STORE
            };
            var created = await Api.CreateSale(saleDto, CancellationToken.None);

            created.Should().NotBeNull();
            created.Id.Should().NotBe(Guid.Empty);
            created.CustomerId.Should().Be(customer.Id);
            created.SoldByEmployeeId.Should().Be(employee.Id);
            created.ProductId.Should().Be(product.Id);
            created.SalePrice.Should().Be("550.00");
            created.Status.Should().Be(SaleStatus.FULFILLED);

            // Read the sale by ID
            var fetched = await Api.GetSaleById(created.Id, CancellationToken.None);
            fetched.Should().NotBeNull();
            fetched.Id.Should().Be(created.Id);
            fetched.CustomerId.Should().Be(customer.Id);

            // Query sales by date range
            var sales = await Api.GetSalesByDateRange(null, null, null, CancellationToken.None);
            sales.Should().ContainSingle();
            sales.First().Id.Should().Be(created.Id);
        }

        /// <summary>
        /// Simple happy path: Create a sale and verify it appears in the list
        /// </summary>
        [Test]
        public async Task CreateSale_ThenGetSales_ShouldReturnSale()
        {
            // Set up required dependencies
            var customer = await Api.CreateCustomer(new CustomerDto { Name = "Jane Doe" }, CancellationToken.None);
            var employee = await Api.CreateEmployee(new EmployeeDto
            {
                Name = "Bob Sales",
                Location = Location.STORE,
                Roles = new List<EmployeeRole> { EmployeeRole.SALESPERSON }
            }, CancellationToken.None);
            var product = await Api.CreateProduct(new ProductDto
            {
                Name = "Mountain Bike",
                ProductType = "Bike",
                Description = "All-terrain mountain bike",
                Supplier = "Trek",
                CostPrice = "500.00",
                RetailPrice = "699.99",
                CommissionPercentage = "5.00"
            }, CancellationToken.None);

            // Create a sale
            var saleDto = new CreateSaleDto
            {
                CustomerId = customer.Id,
                SoldByEmployeeId = employee.Id,
                ProductId = product.Id,
                SaleDate = DateTimeOffset.Now,
                SalePrice = "650.00",
                Location = Location.STORE
            };
            await Api.CreateSale(saleDto, CancellationToken.None);

            // Get sales and verify it's there
            var sales = await Api.GetSalesByDateRange(null, null, null, CancellationToken.None);
            sales.Should().ContainSingle();
            var sale = sales.First();
            sale.CustomerId.Should().Be(customer.Id);
            sale.SoldByEmployeeId.Should().Be(employee.Id);
            sale.ProductId.Should().Be(product.Id);
        }

        /// <summary>
        /// Test that validates sale status is PENDING when no inventory exists
        /// </summary>
        [Test]
        public async Task CreateSale_WithoutInventory_ShouldHavePendingStatus()
        {
            // Set up required dependencies
            var customer = await Api.CreateCustomer(new CustomerDto { Name = "Pending Customer" }, CancellationToken.None);
            var employee = await Api.CreateEmployee(new EmployeeDto
            {
                Name = "Sales Rep",
                Location = Location.STORE,
                Roles = new List<EmployeeRole> { EmployeeRole.SALESPERSON }
            }, CancellationToken.None);
            var product = await Api.CreateProduct(new ProductDto
            {
                Name = "Out of Stock Bike",
                ProductType = "Bike",
                Description = "Currently unavailable bike",
                Supplier = "Future Bikes",
                CostPrice = "600.00",
                RetailPrice = "899.99",
                CommissionPercentage = "6.00"
            }, CancellationToken.None);

            // DO NOT add inventory - this product has no stock

            // Create a sale without inventory
            var saleDto = new CreateSaleDto
            {
                CustomerId = customer.Id,
                SoldByEmployeeId = employee.Id,
                ProductId = product.Id,
                SaleDate = DateTimeOffset.Now,
                SalePrice = "850.00",
                Location = Location.STORE
            };
            var created = await Api.CreateSale(saleDto, CancellationToken.None);

            // Verify the sale was created with PENDING status
            created.Should().NotBeNull();
            created.Status.Should().Be(SaleStatus.PENDING, "Sale should be PENDING when no inventory exists");
            created.CustomerId.Should().Be(customer.Id);
            created.SoldByEmployeeId.Should().Be(employee.Id);
            created.ProductId.Should().Be(product.Id);
        }
    }
}
