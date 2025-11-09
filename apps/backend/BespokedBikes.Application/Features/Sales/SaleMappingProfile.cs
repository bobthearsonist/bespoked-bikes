using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Features.Sales;

/// <summary>
/// AutoMapper profile for Sale entity mappings
/// </summary>
public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => new DateTimeOffset(src.SaleDate, TimeSpan.Zero)))
            .ForMember(dest => dest.FulfilledDate, opt => opt.MapFrom(src => src.FulfilledDate.HasValue ? new DateTimeOffset(src.FulfilledDate.Value, TimeSpan.Zero) : (DateTimeOffset?)null));

        CreateMap<SaleDto, Sale>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate.UtcDateTime))
            .ForMember(dest => dest.FulfilledDate, opt => opt.MapFrom(src => src.FulfilledDate.HasValue ? src.FulfilledDate.Value.UtcDateTime : (DateTime?)null));
    }
}
