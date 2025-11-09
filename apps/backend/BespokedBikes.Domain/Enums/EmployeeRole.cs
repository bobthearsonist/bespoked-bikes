namespace BespokedBikes.Domain.Enums;

[Flags]
public enum EmployeeRole
{
    None = 0,
    Salesperson = 1,
    Fulfillment = 2,
    Admin = 4
}
