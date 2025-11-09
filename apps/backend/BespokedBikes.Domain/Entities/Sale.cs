using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SoldByEmployeeId { get; set; }
    public Guid? FulfilledByEmployeeId { get; set; }
    public Guid ProductId { get; set; }
    public SaleStatus Status { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CommissionAmount { get; set; }
    public required string SaleChannel { get; set; }
    public Location Location { get; set; }
    public DateTime SaleDate { get; set; }
    public DateTime? FulfilledDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
