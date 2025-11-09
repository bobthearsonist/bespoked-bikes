using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Application.Features.Products;
using BespokedBikes.Infrastructure;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Data.Factories;
using BespokedBikes.Infrastructure.Features.Employees;
using BespokedBikes.Infrastructure.Features.Products;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders()
    .AddConsole()
    .AddDebug();

// Create in-memory SQLite factory for development/demo
var dbFactory = new InMemorySqliteDbContextFactory();

// Add infrastructure services with the factory and configure migrations
// Pass the connection string - FluentMigrator will create its own connection
// but since it's the same connection string, SQLite in-memory semantics mean
// we need to keep the original connection open (which the factory does)
builder.Services.AddInfrastructure(
    dbFactory,
    rb => rb
        .AddSQLite()
        .WithGlobalConnectionString(dbFactory.ConnectionString)
        .ScanIn(typeof(InitialCreate).Assembly).For.Migrations());

// Add application services
builder.Services.AddScoped<IEmployeeRoleService, FlagBasedEmployeeRoleService>();

// Add Product services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductMappingProfile>();
});

// Register controller implementation
builder.Services.AddScoped<BespokedBikes.Api.Controllers.IController, BespokedBikes.Api.Controllers.BespokedBikesController>();

// Add API services
builder.Services.AddControllers();
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

// Run database migrations
app.Services.RunMigrations();

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

app.Run();
