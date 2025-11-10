using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Employees;

public class EmployeeService(IEmployeeRepository repository, IMapper mapper) : IEmployeeService
{
    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
    {
        var employee = mapper.Map<Employee>(employeeDto);
        employee.Id = Guid.NewGuid();

        var createdEmployee = await repository.CreateAsync(employee);

        return mapper.Map<EmployeeDto>(createdEmployee);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await repository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IReadOnlyList<EmployeeDto>> ListEmployeesAsync()
    {
        var employees = await repository.GetAllAsync();

        return mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }
}
