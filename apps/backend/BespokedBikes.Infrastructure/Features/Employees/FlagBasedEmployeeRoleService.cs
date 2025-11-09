using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Infrastructure.Features.Employees;

/// <summary>
/// Implementation of IEmployeeRoleService using bit flags.
/// This provides an efficient role management system that can later be
/// replaced with a table-based implementation without changing business logic.
/// </summary>
public class FlagBasedEmployeeRoleService : IEmployeeRoleService
{
    public bool HasRole(Employee employee, EmployeeRole role)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        return employee.Roles.HasFlag(role);
    }

    public bool HasRole(Employee employee, string roleName)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException("Role name cannot be empty.", nameof(roleName));

        if (!Enum.TryParse<EmployeeRole>(roleName, ignoreCase: true, out var role))
            throw new ArgumentException($"Invalid role name: {roleName}", nameof(roleName));

        return HasRole(employee, role);
    }

    public void AddRole(Employee employee, EmployeeRole role)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (role == EmployeeRole.None)
            return;

        employee.Roles |= role;
    }

    public void RemoveRole(Employee employee, EmployeeRole role)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (role == EmployeeRole.None)
            return;

        employee.Roles &= ~role;
    }

    public IEnumerable<EmployeeRole> GetRoles(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (employee.Roles == EmployeeRole.None)
            return Enumerable.Empty<EmployeeRole>();

        return Enum.GetValues<EmployeeRole>()
            .Where(role => role != EmployeeRole.None && employee.Roles.HasFlag(role));
    }

    public void SetRoles(Employee employee, IEnumerable<EmployeeRole> roles)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (roles == null)
            throw new ArgumentNullException(nameof(roles));

        // Clear all roles first
        employee.Roles = EmployeeRole.None;

        // Add each role
        foreach (var role in roles.Where(r => r != EmployeeRole.None))
        {
            employee.Roles |= role;
        }
    }

    public IEnumerable<string> GetRoleNames(Employee employee)
    {
        return GetRoles(employee).Select(r => r.ToString());
    }
}
