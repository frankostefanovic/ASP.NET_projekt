using Lab2.RezervacijeProstora;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab2.RezervacijeProstora.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var dbContextDescriptors = services
                    .Where(descriptor =>
                        descriptor.ServiceType == typeof(ApplicationDbContext) ||
                        descriptor.ServiceType == typeof(DbContextOptions) ||
                        descriptor.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                        descriptor.ServiceType.FullName?.Contains("IDbContextOptionsConfiguration") == true)
                    .ToList();

                foreach (var descriptor in dbContextDescriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });
            });
        }
    }
}
