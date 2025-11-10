using AutoMapper;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Generated;

/// <summary>
/// Partial class to configure AutoMapper mapping for ProductDto.
/// Uses convention-based mapping with ReverseMap for bidirectional mapping.
/// </summary>
[AutoMap(typeof(Product), ReverseMap = true)]
public partial class ProductDto
{
}
