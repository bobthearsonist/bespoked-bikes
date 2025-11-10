using AutoMapper;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;

namespace BespokedBikes.Application.Common.Mapping;

/// <summary>
/// AutoMapper value resolver to convert DateTime to DateTimeOffset for SaleDate
/// </summary>
public class SaleDateToOffsetResolver : IValueResolver<Sale, SaleDto, DateTimeOffset>
{
    public DateTimeOffset Resolve(Sale source, SaleDto destination, DateTimeOffset destMember, ResolutionContext context)
    {
        return new DateTimeOffset(source.SaleDate, TimeSpan.Zero);
    }
}

/// <summary>
/// AutoMapper value resolver to convert DateTimeOffset back to DateTime for SaleDate
/// </summary>
public class SaleDateToDateTimeResolver : IValueResolver<SaleDto, Sale, DateTime>
{
    public DateTime Resolve(SaleDto source, Sale destination, DateTime destMember, ResolutionContext context)
    {
        return source.SaleDate.UtcDateTime;
    }
}

/// <summary>
/// AutoMapper value resolver to convert nullable DateTime to nullable DateTimeOffset for FulfilledDate
/// </summary>
public class FulfilledDateToOffsetResolver : IValueResolver<Sale, SaleDto, DateTimeOffset?>
{
    public DateTimeOffset? Resolve(Sale source, SaleDto destination, DateTimeOffset? destMember, ResolutionContext context)
    {
        return source.FulfilledDate.HasValue
            ? new DateTimeOffset(source.FulfilledDate.Value, TimeSpan.Zero)
            : null;
    }
}

/// <summary>
/// AutoMapper value resolver to convert nullable DateTimeOffset back to nullable DateTime for FulfilledDate
/// </summary>
public class FulfilledDateToDateTimeResolver : IValueResolver<SaleDto, Sale, DateTime?>
{
    public DateTime? Resolve(SaleDto source, Sale destination, DateTime? destMember, ResolutionContext context)
    {
        return source.FulfilledDate?.UtcDateTime;
    }
}
