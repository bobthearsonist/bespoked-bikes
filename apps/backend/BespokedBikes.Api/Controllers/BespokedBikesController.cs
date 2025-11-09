using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Generated;
using Microsoft.AspNetCore.Mvc;

namespace BespokedBikes.Api.Controllers;

//TODO this pattern kind of sucks. i would like to have individual controllers files.

/// <summary>
/// Implementation of IController interface that delegates to feature services.
/// </summary>
public class BespokedBikesControllerImplementation(
    IProductService productService,
    IEmployeeService employeeService,
    ICustomerService customerService,
    IInventoryService inventoryService)
    : IController
{
    // Product endpoints
    public async Task<ProductDto> CreateProductAsync(ProductDto body)
    {
        return await productService.CreateProductAsync(body);
    }

    public async Task<ICollection<ProductDto>> ListProductsAsync()
    {
        return await productService.ListProductsAsync();
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        return await productService.GetProductAsync(id);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, ProductDto body)
    {
        return await productService.UpdateProductAsync(id, body);
    }

    public async Task<InventoryDto> UpdateProductInventoryAsync(Guid id, InventoryUpdateDto body)
    {
        return await inventoryService.UpdateProductInventoryAsync(id, body);
    }

    // Customer endpoints
    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto body)
    {
        return await customerService.CreateCustomerAsync(body);
    }

    public async Task<ICollection<CustomerDto>> SearchCustomersAsync(string? searchTerm)
    {
        // For MVP, ignore searchTerm and return all customers
        // Search functionality punted for simplification
        var customers = await customerService.GetAllCustomersAsync();
        return customers.ToList();
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }
        return customer;
    }

    public async Task<CustomerDto> UpdateCustomerAsync(Guid id, CustomerDto body)
    {
        return await customerService.UpdateCustomerAsync(id, body);
    }

    // Employee endpoints
    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto body)
    {
        return await employeeService.CreateEmployeeAsync(body);
    }

    public async Task<ICollection<EmployeeDto>> ListEmployeesAsync(Location? location, EmployeeRole? role)
    {
        // For MVP, ignore filters and return all employees
        // TODO use ODATA for filtering, sorting, paging etc.
        var employees = await employeeService.ListEmployeesAsync();
        return employees.ToList();
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        return employee;
    }

    public Task<EmployeeDto> UpdateEmployeeAsync(Guid id, EmployeeDto body)
    {
        throw new NotImplementedException("Employee update not yet implemented");
    }

    // Sales endpoints
    public Task<SaleDto> CreateSaleAsync(CreateSaleDto body)
    {
        throw new NotImplementedException("Sales management not yet implemented");
    }

    public Task<ICollection<SaleDto>> GetSalesByDateRangeAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, SaleStatus? status)
    {
        throw new NotImplementedException("Sales management not yet implemented");
    }

    public Task<SaleDto> GetSaleByIdAsync(Guid id)
    {
        throw new NotImplementedException("Sales management not yet implemented");
    }

    // Reporting endpoints
    public Task<QuarterlyCommissionReportDto> GetQuarterlyCommissionsAsync(int year, int quarter)
    {
        throw new NotImplementedException("Reporting not yet implemented");
    }

    public Task<EmployeeCommissionDto> GetEmployeeQuarterlyCommissionsAsync(Guid id, int year, int quarter)
    {
        throw new NotImplementedException("Reporting not yet implemented");
    }
}
