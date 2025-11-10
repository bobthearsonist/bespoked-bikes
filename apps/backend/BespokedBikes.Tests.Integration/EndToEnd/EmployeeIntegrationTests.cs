using FluentAssertions;
using BespokedBikes.Tests.Integration.Generated;
using Refit;
using System.Net;
using Location = BespokedBikes.Application.Generated.Location;
using EmployeeRole = BespokedBikes.Application.Generated.EmployeeRole;
using EmployeeDto = BespokedBikes.Application.Generated.EmployeeDto;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [TestFixture]
    public class EmployeeIntegrationTests : IntegrationTestBase
    {
        /// <summary>
        /// Happy path: Create an employee and verify it appears in the list
        ///
        /// TODO: Failing with 400 - need to add AddNewtonsoftJson() to Program.cs for model validation
        /// </summary>
        [Test]
        public async Task CreateEmployee_ThenList_ShouldReturnEmployee()
        {
            // Create an employee
            var employeeDto = new EmployeeDto
            {
                Name = "Jane Tech",
                Location = Location.STORE,
                Roles = new List<EmployeeRole> { EmployeeRole.SALESPERSON }
            };
            await Api.CreateEmployee(employeeDto, CancellationToken.None);

            // List employees and verify it's there
            var employees = await Api.ListEmployees(null, null, CancellationToken.None);
            employees.Should().ContainSingle();
            var employee = employees.First();
            employee.Name.Should().Be("Jane Tech");
            employee.Location.Should().Be(Location.STORE);
            employee.Roles.Should().Contain(EmployeeRole.SALESPERSON);
        }
    }
}
