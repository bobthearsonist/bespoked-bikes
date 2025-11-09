using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;
using System.Globalization;

namespace BespokedBikes.Application.Features.Products;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<decimal, string>().ConvertUsing(d => d.ToString("F2", CultureInfo.InvariantCulture));
        CreateMap<string, decimal>().ConvertUsing(s => decimal.Parse(s, CultureInfo.InvariantCulture));

        CreateMap<Product, ProductDto>();

        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
