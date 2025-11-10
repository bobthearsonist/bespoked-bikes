using BespokedBikes.Application.Common;
using BespokedBikes.Application.Features.Customers;
using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Inventory;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Application.Features.Sales;
using BespokedBikes.Application.Generated;
using BespokedBikes.Infrastructure;
using BespokedBikes.Infrastructure.Features.Customers;
using BespokedBikes.Infrastructure.Features.Employees;
using BespokedBikes.Infrastructure.Features.Inventory;
using BespokedBikes.Infrastructure.Features.Products;
using BespokedBikes.Infrastructure.Features.Sales;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders()
    .AddConsole()
    .AddDebug();

// Add infrastructure services with automatic database provider selection
builder.Services.AddInfrastructure(builder.Configuration);

//TODO move to extension methods to keep this cleaner and easier to read
//TODO after that we could even look at using source-generated DI with attributes and Microsoft.Extensions.DependencyInjection.SourceGeneration so we dont have to declare them here at all (sounds alot like autofac))

// Add Product services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add Customer services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Add Inventory services
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Add Sales services
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISalesService, SalesService>();

// Add Employee services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRoleService, FlagBasedEmployeeRoleService>();

// Add AutoMapper with assembly scanning and global type converters
builder.Services.AddAutoMapper(config =>
{
    // Configure global decimal <-> string converters for monetary values
    // This works for our current spec, it could be done a handful of other ways if there were conflicts
    config.CreateMap<decimal, string>()
        .ConvertUsing(d => d.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
    config.CreateMap<string, decimal>()
        .ConvertUsing(s => string.IsNullOrEmpty(s) ? 0m : decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture));
    config.CreateMap<string?, decimal>()
        .ConvertUsing(s => string.IsNullOrEmpty(s) ? 0m : decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture));

    // Scan assemblies for [AutoMap] attributes
    config.AddMaps(typeof(ProductDto).Assembly);
});

// Register controller implementation
builder.Services.AddScoped<BespokedBikes.Api.Controllers.IController, BespokedBikes.Api.Controllers.BespokedBikesControllerImplementation>();

// Add API services
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => JsonSerializerConfiguration.ConfigureNewtonsoftJson(options.SerializerSettings));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// TODO clean this up with an extension method
// Run schema migrations
app.Services.RunMigrations();
// Run test data migrations based on configuration
if (app.Configuration.GetValue("TestData:SeedOnStartup", false)) app.Services.RunTestDataMigrations();

// Map Scalar endpoints only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Add health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();

// Public partial class declaration for integration testing with WebApplicationFactory
public partial class Program { }
