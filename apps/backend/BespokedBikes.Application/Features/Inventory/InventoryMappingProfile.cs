using AutoMapper;
using BespokedBikes.Application.Generated;

namespace BespokedBikes.Application.Features.Inventory;

/// <summary>
/// AutoMapper profile for Inventory entity mappings
/// </summary>
public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<Domain.Entities.Inventory, InventoryDto>();
        CreateMap<InventoryDto, Domain.Entities.Inventory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
