using BespokedBikes.Application.Features.Employees;
using BespokedBikes.Infrastructure.Data;
using BespokedBikes.Infrastructure.Features.Employees;
using BespokedBikes.Infrastructure.Migrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders()
    .AddConsole()
    .AddDebug();

// SQLite in-memory connection string
var connectionString = "DataSource=:memory:";

// Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Add FluentMigrator services for SQLite
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(InitialCreate).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

// Add application services
builder.Services.AddScoped<IEmployeeRoleService, FlagBasedEmployeeRoleService>();

// Add services
builder.Services
    .AddControllers();
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

// Run FluentMigrator migrations for SQLite
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

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
