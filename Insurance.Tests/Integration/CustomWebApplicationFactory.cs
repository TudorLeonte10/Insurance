
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection _connection = null!;
    public Guid SeededCityId { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>))
                .ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<InsuranceDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
        });
    }

    public void SeedTestData(InsuranceDbContext db)
    {
        var country = new CountryEntity { Name = "Romania" };
        var county = new CountyEntity { Name = "Iasi", Country = country };
        var city = new CityEntity { Name = "Pascani", County = county };

        db.AddRange(country, county, city);
        db.SaveChanges();

        SeededCityId = city.Id;
    }
}
