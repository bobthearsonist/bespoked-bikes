using BespokedBikes.Domain.Entities;
using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Application.Features.Employees;

/// <summary>
/// Service for managing employee roles with an abstraction that allows
/// switching between bit flag implementation and a roles table in the future.
/// </summary>
public interface IEmployeeRoleService
{
    bool HasRole(Employee employee, EmployeeRole role);

    bool HasRole(Employee employee, string roleName);

    void AddRole(Employee employee, EmployeeRole role);

    void RemoveRole(Employee employee, EmployeeRole role);

    IEnumerable<EmployeeRole> GetRoles(Employee employee);

    void SetRoles(Employee employee, IEnumerable<EmployeeRole> roles);
    IEnumerable<string> GetRoleNames(Employee employee);
}
