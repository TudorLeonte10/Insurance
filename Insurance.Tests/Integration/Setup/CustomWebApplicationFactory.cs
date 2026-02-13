
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Entities;
using Insurance.Tests.Integration.Setup;
using Microsoft.AspNetCore.Authentication;
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
    public Guid SeededCurrencyId { get; private set; }
    public Guid SeededBrokerId { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName, options => { });

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

        var currency = new CurrencyEntity
        {
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1m,
            IsActive = true
        };

        var broker = new BrokerEntity
        {
            BrokerCode = "BR-TEST",
            Name = "Seed Broker",
            Email = "seedbroker@test.local",
            Phone = "0700000000",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        db.AddRange(country, county, city, currency, broker);
        db.SaveChanges();

        SeededCityId = city.Id;
        SeededCurrencyId = currency.Id;
        SeededBrokerId = broker.Id;
    }
}
