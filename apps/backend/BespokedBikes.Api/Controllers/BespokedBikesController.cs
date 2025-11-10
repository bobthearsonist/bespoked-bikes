using AutoMapper;
using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Application.Generated;
using BespokedBikes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BespokedBikes.Api.Controllers;

//TODO this pattern kind of sucks. i would like to have individual controllers files.

/// <summary>
/// Implementation of IController interface that delegates to feature services.
/// Returns proper HTTP status codes using ActionResult.
/// Handles mapping between DTOs and domain entities.
/// </summary>
public class BespokedBikesControllerImplementation(
    IProductService productService,
    IEmployeeService employeeService,
    ICustomerService customerService,
    IInventoryService inventoryService,
    ISalesService salesService,
    IMapper mapper)
    : ControllerBase, IController
{
    // Product endpoints
    public async Task<ActionResult<ProductDto>> CreateProductAsync(ProductDto body)
    {
        var productEntity = mapper.Map<Product>(body);
        var createdProduct = await productService.CreateProductAsync(productEntity);
        var productDto = mapper.Map<ProductDto>(createdProduct);
        return new CreatedResult($"/api/products/{productDto.Id}", productDto);
    }

    public async Task<ActionResult<ICollection<ProductDto>>> ListProductsAsync()
    {
        var products = await productService.ListProductsAsync();
        var productDtos = mapper.Map<ICollection<ProductDto>>(products);
        return Ok(productDtos);
    }

    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await productService.GetProductAsync(id);
        var productDto = mapper.Map<ProductDto>(product);
        return Ok(productDto);
    }

    public async Task<ActionResult<ProductDto>> UpdateProductAsync(Guid id, ProductDto body)
    {
        var productEntity = mapper.Map<Product>(body);
        var updatedProduct = await productService.UpdateProductAsync(id, productEntity);
        var productDto = mapper.Map<ProductDto>(updatedProduct);
        return Ok(productDto);
    }

    public async Task<ActionResult<InventoryDto>> UpdateProductInventoryAsync(Guid id, InventoryUpdateDto body)
    {
        var location = (Domain.Enums.Location)body.Location;
        var inventory = await inventoryService.UpdateProductInventoryAsync(id, location, body.Quantity);
        var inventoryDto = mapper.Map<InventoryDto>(inventory);
        return Ok(inventoryDto);
    }

    // Customer endpoints
    public async Task<ActionResult<CustomerDto>> CreateCustomerAsync(CustomerDto body)
    {
        var customerEntity = mapper.Map<Customer>(body);
        var createdCustomer = await customerService.CreateCustomerAsync(customerEntity);
        var customerDto = mapper.Map<CustomerDto>(createdCustomer);
        return new CreatedResult($"/api/customers/{customerDto.Id}", customerDto);
    }

    public async Task<ActionResult<ICollection<CustomerDto>>> SearchCustomersAsync(string? searchTerm = null)
    {
        // For MVP, ignore searchTerm and return all customers
        // Search functionality punted for simplification
        var customers = await customerService.GetAllCustomersAsync();
        var customerDtos = mapper.Map<ICollection<CustomerDto>>(customers.ToList());
        return Ok(customerDtos);
    }

    public async Task<ActionResult<CustomerDto>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        var customerDto = mapper.Map<CustomerDto>(customer);
        return Ok(customerDto);
    }

    public async Task<ActionResult<CustomerDto>> UpdateCustomerAsync(Guid id, CustomerDto body)
    {
        var customerEntity = mapper.Map<Customer>(body);
        var updatedCustomer = await customerService.UpdateCustomerAsync(id, customerEntity);
        var customerDto = mapper.Map<CustomerDto>(updatedCustomer);
        return Ok(customerDto);
    }

    // Employee endpoints
    public async Task<ActionResult<EmployeeDto>> CreateEmployeeAsync(EmployeeDto body)
    {
        var employeeEntity = mapper.Map<Employee>(body);
        var createdEmployee = await employeeService.CreateEmployeeAsync(employeeEntity);
        var employeeDto = mapper.Map<EmployeeDto>(createdEmployee);
        return new CreatedResult($"/api/employees/{employeeDto.Id}", employeeDto);
    }

    public async Task<ActionResult<ICollection<EmployeeDto>>> ListEmployeesAsync(Location? location = null, EmployeeRole? role = null)
    {
        // For MVP, ignore filters and return all employees
        // TODO use ODATA for filtering, sorting, paging etc.
        var employees = await employeeService.ListEmployeesAsync();
        var employeeDtos = mapper.Map<ICollection<EmployeeDto>>(employees.ToList());
        return Ok(employeeDtos);
    }

    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        var employeeDto = mapper.Map<EmployeeDto>(employee);
        return Ok(employeeDto);
    }

    public Task<ActionResult<EmployeeDto>> UpdateEmployeeAsync(Guid id, EmployeeDto body)
    {
        throw new NotImplementedException("Employee update not yet implemented");
    }

    // Sales endpoints
    public async Task<ActionResult<SaleDto>> CreateSaleAsync(CreateSaleDto body)
    {
        var saleEntity = mapper.Map<Sale>(body);
        var createdSale = await salesService.CreateSaleAsync(saleEntity);
        var saleDto = mapper.Map<SaleDto>(createdSale);
        return new CreatedResult($"/api/sales/{saleDto.Id}", saleDto);
    }

    public async Task<ActionResult<ICollection<SaleDto>>> GetSalesByDateRangeAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, SaleStatus? status = null)
    {
        // For MVP, ignore filters and return all sales
        // TODO: Implement proper filtering with OData
        var domainStatus = status.HasValue ? (Domain.Enums.SaleStatus)status.Value : (Domain.Enums.SaleStatus?)null;
        var startDateTime = startDate?.UtcDateTime;
        var endDateTime = endDate?.UtcDateTime;

        var sales = await salesService.GetSalesByDateRangeAsync(startDateTime, endDateTime, domainStatus);
        var saleDtos = mapper.Map<ICollection<SaleDto>>(sales.ToList());
        return Ok(saleDtos);
    }

    public async Task<ActionResult<SaleDto>> GetSaleByIdAsync(Guid id)
    {
        var sale = await salesService.GetSaleByIdAsync(id);
        var saleDto = mapper.Map<SaleDto>(sale);
        return Ok(saleDto);
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
