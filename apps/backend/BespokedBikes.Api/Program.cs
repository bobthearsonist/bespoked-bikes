var builder = WebApplication.CreateBuilder(args);

// Configure logging using Microsoft.Extensions.Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register application services (to be implemented)
// builder.Services.AddScoped<ICustomerService, CustomerService>();
// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<ISalesService, SalesService>();
// builder.Services.AddScoped<IReportingService, ReportingService>();

// Register AutoMapper (to be implemented)
// builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register FluentValidation (to be implemented)
// builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

// Register DbContext (to be implemented)
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// OpenAPI and Scalar configuration (to be implemented)

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // OpenAPI and Scalar endpoints to be configured
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
