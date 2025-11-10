using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Common.Mapping;

/// <summary>
/// AutoMapper value resolver to convert between Flags enum (efficient storage) and collection (clean API)
/// </summary>
public class EmployeeRolesToCollectionResolver : IValueResolver<Employee, EmployeeDto, ICollection<EmployeeRole>>
{
    public ICollection<EmployeeRole> Resolve(Employee source, EmployeeDto destination, ICollection<EmployeeRole> destMember, ResolutionContext context)
    {
        var result = new List<EmployeeRole>();

        if (source.Roles.HasFlag(Domain.Enums.EmployeeRole.Salesperson))
            result.Add(EmployeeRole.SALESPERSON);
        if (source.Roles.HasFlag(Domain.Enums.EmployeeRole.Fulfillment))
            result.Add(EmployeeRole.FULFILLMENT);
        if (source.Roles.HasFlag(Domain.Enums.EmployeeRole.Admin))
            result.Add(EmployeeRole.ADMIN);

        return result;
    }
}

/// <summary>
/// AutoMapper value resolver to convert collection back to Flags enum
/// </summary>
public class EmployeeRolesToFlagsResolver : IValueResolver<EmployeeDto, Employee, Domain.Enums.EmployeeRole>
{
    public Domain.Enums.EmployeeRole Resolve(EmployeeDto source, Employee destination, Domain.Enums.EmployeeRole destMember, ResolutionContext context)
    {
        var flags = Domain.Enums.EmployeeRole.None;

        foreach (var role in source.Roles)
        {
            flags |= role switch
            {
                EmployeeRole.SALESPERSON => Domain.Enums.EmployeeRole.Salesperson,
                EmployeeRole.FULFILLMENT => Domain.Enums.EmployeeRole.Fulfillment,
                EmployeeRole.ADMIN => Domain.Enums.EmployeeRole.Admin,
                _ => Domain.Enums.EmployeeRole.None
            };
        }

        return flags;
    }
}
