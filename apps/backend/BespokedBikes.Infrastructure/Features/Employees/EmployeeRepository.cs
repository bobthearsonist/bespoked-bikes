using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BespokedBikes.Infrastructure.Features.Employees;

public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
{
    public async Task<Employee> CreateAsync(Employee employee)
    {
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<Employee>> GetAllAsync()
    {
        return await context.Employees
            .OrderBy(e => e.Name)
            .ToListAsync();
    }
}
