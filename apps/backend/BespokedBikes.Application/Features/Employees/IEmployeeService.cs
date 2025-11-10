using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Employees;

public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Employee?> GetEmployeeByIdAsync(Guid id);
    Task<IReadOnlyList<Employee>> ListEmployeesAsync();
}
