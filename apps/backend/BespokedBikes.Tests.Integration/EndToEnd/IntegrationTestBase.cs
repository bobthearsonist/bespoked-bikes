using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using BespokedBikes.Tests.Integration.Generated;

namespace BespokedBikes.Tests.Integration.EndToEnd
{
    [Category("Integration")]
    public abstract class IntegrationTestBase
    {
        private WebApplicationFactory<Program> Factory { get; set; } = null!;
        protected HttpClient Client { get; set; } = null!;
        private string DatabaseName { get; set; } = null!;

        protected IIBespokedBikesApiApi Api = null!;

        protected static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            DatabaseName = $"IntegrationTest_{Guid.NewGuid()}";

            Factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace the database context with an in-memory one for testing
                        RemoveDbContext<BespokedBikes.Infrastructure.Data.ApplicationDbContext>(services);

                        services.AddDbContext<BespokedBikes.Infrastructure.Data.ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(DatabaseName);
                            options.EnableSensitiveDataLogging();
                        });
                    });
                });

            Client = Factory.CreateClient();
            Api = CreateApi<IIBespokedBikesApiApi>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            // Clean up the database after each test
            // TODO use transactions for a much faster cleanup. might have to use the test container approach for transactions though.
            using var scope = Factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BespokedBikes.Infrastructure.Data.ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Client.Dispose();
            Factory.Dispose();
        }

        protected T CreateApi<T>() where T : class
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(JsonSettings)
            };
            return RestService.For<T>(Client, refitSettings);
        }

        private static void RemoveDbContext<TContext>(IServiceCollection services)
            where TContext : DbContext
        {
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<TContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(TContext)).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }
    }
}
