using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Employees;

public interface IEmployeeRepository
{
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Employee>> GetAllAsync();
}
