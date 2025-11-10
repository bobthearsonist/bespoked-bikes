using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Employees;

public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        employee.Id = Guid.NewGuid();
        return await repository.CreateAsync(employee);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await repository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        return employee;
    }

    public async Task<IReadOnlyList<Employee>> ListEmployeesAsync()
    {
        return await repository.GetAllAsync();
    }
}
