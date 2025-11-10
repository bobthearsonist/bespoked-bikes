using FluentAssertions;
using CustomerDto = BespokedBikes.Application.Generated.CustomerDto;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [TestFixture]
    public class CustomerIntegrationTests : IntegrationTestBase
    {
        /// <summary>
        /// Happy path: Create a customer and verify it appears in the list
        /// </summary>
        [Test]
        public async Task CreateCustomer_ThenList_ShouldReturnCustomer()
        {
            // Create a customer
            var customerDto = new CustomerDto { Name = "Jane Smith" };
            await Api.CreateCustomer(customerDto, CancellationToken.None);

            // List customers and verify it's there
            var customers = await Api.SearchCustomers(null, CancellationToken.None);
            customers.Should().ContainSingle();
            customers.First().Name.Should().Be("Jane Smith");
        }
    }
}
