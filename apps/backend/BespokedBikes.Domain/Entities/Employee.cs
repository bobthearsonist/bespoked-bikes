using BespokedBikes.Domain.Enums;

namespace BespokedBikes.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Location Location { get; set; }
    public EmployeeRole Roles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
