namespace BespokedBikes.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string ProductType { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Supplier { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal CommissionPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
