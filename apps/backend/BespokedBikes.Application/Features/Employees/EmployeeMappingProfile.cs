using AutoMapper;
using BespokedBikes.Domain.Entities;
using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Employees;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        // Convert between Flags enum (efficient storage) and array (clean API)
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                ConvertFlagsToCollection(src.Roles)));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                ConvertCollectionToFlags(src.Roles)));
    }

    private static List<EmployeeRole> ConvertFlagsToCollection(Domain.Enums.EmployeeRole flags)
    {
        var result = new List<EmployeeRole>();

        if (flags.HasFlag(Domain.Enums.EmployeeRole.Salesperson))
            result.Add(EmployeeRole.SALESPERSON);
        if (flags.HasFlag(Domain.Enums.EmployeeRole.Fulfillment))
            result.Add(EmployeeRole.FULFILLMENT);
        if (flags.HasFlag(Domain.Enums.EmployeeRole.Admin))
            result.Add(EmployeeRole.ADMIN);

        return result;
    }

    private static Domain.Enums.EmployeeRole ConvertCollectionToFlags(ICollection<EmployeeRole> roles)
    {
        var flags = Domain.Enums.EmployeeRole.None;

        foreach (var role in roles)
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
