using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Application.Generated;
using Microsoft.AspNetCore.Mvc;

namespace BespokedBikes.Api.Controllers;

//TODO this pattern kind of sucks. i would like to have individual controllers files.

/// <summary>
/// Implementation of IController interface that delegates to feature services.
/// Returns proper HTTP status codes using ActionResult.
/// </summary>
public class BespokedBikesControllerImplementation(
    IProductService productService,
    IEmployeeService employeeService,
    ICustomerService customerService,
    IInventoryService inventoryService,
    ISalesService salesService)
    : ControllerBase, IController
{
    // Product endpoints
    public async Task<ActionResult<ProductDto>> CreateProductAsync(ProductDto body)
    {
        var product = await productService.CreateProductAsync(body);
        return new CreatedResult($"/api/products/{product.Id}", product);
    }

    public async Task<ActionResult<ICollection<ProductDto>>> ListProductsAsync()
    {
        var products = await productService.ListProductsAsync();
        return Ok(products);
    }

    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await productService.GetProductAsync(id);
        return Ok(product);
    }

    public async Task<ActionResult<ProductDto>> UpdateProductAsync(Guid id, ProductDto body)
    {
        var product = await productService.UpdateProductAsync(id, body);
        return Ok(product);
    }

    public async Task<ActionResult<InventoryDto>> UpdateProductInventoryAsync(Guid id, InventoryUpdateDto body)
    {
        var inventory = await inventoryService.UpdateProductInventoryAsync(id, body);
        return Ok(inventory);
    }

    // Customer endpoints
    public async Task<ActionResult<CustomerDto>> CreateCustomerAsync(CustomerDto body)
    {
        var customer = await customerService.CreateCustomerAsync(body);
        return new CreatedResult($"/api/customers/{customer.Id}", customer);
    }

    public async Task<ActionResult<ICollection<CustomerDto>>> SearchCustomersAsync(string? searchTerm = null)
    {
        // For MVP, ignore searchTerm and return all customers
        // Search functionality punted for simplification
        var customers = await customerService.GetAllCustomersAsync();
        return Ok(customers.ToList());
    }

    public async Task<ActionResult<CustomerDto>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        return Ok(customer);
    }

    public async Task<ActionResult<CustomerDto>> UpdateCustomerAsync(Guid id, CustomerDto body)
    {
        var customer = await customerService.UpdateCustomerAsync(id, body);
        return Ok(customer);
    }

    // Employee endpoints
    public async Task<ActionResult<EmployeeDto>> CreateEmployeeAsync(EmployeeDto body)
    {
        var employee = await employeeService.CreateEmployeeAsync(body);
        return new CreatedResult($"/api/employees/{employee.Id}", employee);
    }

    public async Task<ActionResult<ICollection<EmployeeDto>>> ListEmployeesAsync(Location? location = null, EmployeeRole? role = null)
    {
        // For MVP, ignore filters and return all employees
        // TODO use ODATA for filtering, sorting, paging etc.
        var employees = await employeeService.ListEmployeesAsync();
        return Ok(employees.ToList());
    }

    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    public Task<ActionResult<EmployeeDto>> UpdateEmployeeAsync(Guid id, EmployeeDto body)
    {
        throw new NotImplementedException("Employee update not yet implemented");
    }

    // Sales endpoints
    public async Task<ActionResult<SaleDto>> CreateSaleAsync(CreateSaleDto body)
    {
        var sale = await salesService.CreateSaleAsync(body);
        return new CreatedResult($"/api/sales/{sale.Id}", sale);
    }

    public async Task<ActionResult<ICollection<SaleDto>>> GetSalesByDateRangeAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, SaleStatus? status = null)
    {
        // For MVP, ignore filters and return all sales
        // TODO: Implement proper filtering with OData
        var sales = await salesService.GetSalesByDateRangeAsync(startDate, endDate, status);
        return Ok(sales.ToList());
    }

    public async Task<ActionResult<SaleDto>> GetSaleByIdAsync(Guid id)
    {
        var sale = await salesService.GetSaleByIdAsync(id);
        return Ok(sale);
    }

    // Reporting endpoints
    public Task<ActionResult<QuarterlyCommissionReportDto>> GetQuarterlyCommissionsAsync(int year, int quarter)
    {
        throw new NotImplementedException("Reporting not yet implemented");
    }

    public Task<ActionResult<EmployeeCommissionDto>> GetEmployeeQuarterlyCommissionsAsync(Guid id, int year, int quarter)
    {
        throw new NotImplementedException("Reporting not yet implemented");
    }
}
