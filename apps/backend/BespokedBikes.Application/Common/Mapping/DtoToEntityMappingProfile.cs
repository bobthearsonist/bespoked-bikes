using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;
using System.Globalization;

namespace BespokedBikes.Application.Common.Mapping;

/// <summary>
/// Consolidated AutoMapper profile for all DTO to Entity and Entity to DTO mappings.
/// Uses convention-based mapping where possible, only configuring special cases.
/// </summary>
public class DtoToEntityMappingProfile : Profile
{
    public DtoToEntityMappingProfile()
    {
        // Global configuration: Ignore CreatedAt and UpdatedAt for all mappings
        ShouldMapProperty = prop => prop.Name != nameof(Product.CreatedAt) && prop.Name != nameof(Product.UpdatedAt);

        // Global type converters for decimal <-> string (for prices/money)
        CreateMap<decimal, string>().ConvertUsing(d => d.ToString("F2", CultureInfo.InvariantCulture));
        CreateMap<string, decimal>().ConvertUsing(s => decimal.Parse(s, CultureInfo.InvariantCulture));

        // Global type converters for DateTime <-> DateTimeOffset
        CreateMap<DateTime, DateTimeOffset>().ConvertUsing(dt => new DateTimeOffset(dt, TimeSpan.Zero));
        CreateMap<DateTimeOffset, DateTime>().ConvertUsing(dto => dto.UtcDateTime);

        // Enum mappings (DTO enums <-> Domain enums with same values)
        CreateMap<Generated.Location, Domain.Enums.Location>().ConvertUsing(src => (Domain.Enums.Location)src);
        CreateMap<Domain.Enums.Location, Generated.Location>().ConvertUsing(src => (Generated.Location)src);
        CreateMap<Generated.SaleStatus, Domain.Enums.SaleStatus>().ConvertUsing(src => (Domain.Enums.SaleStatus)src);
        CreateMap<Domain.Enums.SaleStatus, Generated.SaleStatus>().ConvertUsing(src => (Generated.SaleStatus)src);

        // Simple entity mappings (properties map by convention)
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Inventory, InventoryDto>().ReverseMap();

        // Employee: needs custom role conversion (flags <-> collection)
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.Roles, o => o.MapFrom(src => ConvertRolesToCollection(src.Roles)));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(d => d.Roles, o => o.MapFrom(src => ConvertRolesToFlags(src.Roles)));

        // Sale: needs custom date/time conversion
        CreateMap<Sale, SaleDto>()
            .ForMember(d => d.SaleDate, o => o.MapFrom(src => new DateTimeOffset(src.SaleDate, TimeSpan.Zero)))
            .ForMember(d => d.FulfilledDate, o => o.MapFrom(src =>
                src.FulfilledDate.HasValue ? new DateTimeOffset(src.FulfilledDate.Value, TimeSpan.Zero) : (DateTimeOffset?)null));

        CreateMap<SaleDto, Sale>()
            .ForMember(d => d.SaleDate, o => o.MapFrom(src => src.SaleDate.UtcDateTime))
            .ForMember(d => d.FulfilledDate, o => o.MapFrom(src => src.FulfilledDate.HasValue ? src.FulfilledDate.Value.UtcDateTime : (DateTime?)null));

        CreateMap<CreateSaleDto, Sale>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CustomerId, o => o.MapFrom(s => s.CustomerId))
            .ForMember(d => d.SoldByEmployeeId, o => o.MapFrom(s => s.SoldByEmployeeId))
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
            .ForMember(d => d.SalePrice, o => o.MapFrom(s => decimal.Parse(s.SalePrice, CultureInfo.InvariantCulture)))
            .ForMember(d => d.SaleChannel, o => o.MapFrom(s => s.SaleChannel))
            .ForMember(d => d.Location, o => o.MapFrom(s => (Domain.Enums.Location)s.Location))
            .ForMember(d => d.SaleDate, o => o.MapFrom(s => s.SaleDate.UtcDateTime))
            .ForMember(d => d.CommissionAmount, o => o.Ignore()) // Calculated by service
            .ForMember(d => d.Status, o => o.Ignore()) // Determined by service
            .ForMember(d => d.FulfilledByEmployeeId, o => o.Ignore())
            .ForMember(d => d.FulfilledDate, o => o.Ignore());
    }

    // Helper methods for Employee role conversions
    private static ICollection<Generated.EmployeeRole> ConvertRolesToCollection(Domain.Enums.EmployeeRole flags)
    {
        var result = new List<Generated.EmployeeRole>();

        if (flags.HasFlag(Domain.Enums.EmployeeRole.Salesperson))
            result.Add(Generated.EmployeeRole.SALESPERSON);
        if (flags.HasFlag(Domain.Enums.EmployeeRole.Fulfillment))
            result.Add(Generated.EmployeeRole.FULFILLMENT);
        if (flags.HasFlag(Domain.Enums.EmployeeRole.Admin))
            result.Add(Generated.EmployeeRole.ADMIN);

        return result;
    }

    private static Domain.Enums.EmployeeRole ConvertRolesToFlags(ICollection<Generated.EmployeeRole> roles)
    {
        var flags = Domain.Enums.EmployeeRole.None;

        foreach (var role in roles)
        {
            flags |= role switch
            {
                Generated.EmployeeRole.SALESPERSON => Domain.Enums.EmployeeRole.Salesperson,
                Generated.EmployeeRole.FULFILLMENT => Domain.Enums.EmployeeRole.Fulfillment,
                Generated.EmployeeRole.ADMIN => Domain.Enums.EmployeeRole.Admin,
                _ => Domain.Enums.EmployeeRole.None
            };
        }

        return flags;
    }
}
