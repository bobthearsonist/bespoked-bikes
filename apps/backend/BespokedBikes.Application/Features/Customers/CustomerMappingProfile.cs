using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Customers;

/// <summary>
/// AutoMapper profile for Customer entity and CustomerDto
/// Ignores timestamp fields (CreatedAt, UpdatedAt) when mapping from DTO to entity
/// as these are managed by the repository layer
/// </summary>
public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();

        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
