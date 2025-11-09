using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Employees;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
    Task<IReadOnlyList<EmployeeDto>> ListEmployeesAsync();
}
