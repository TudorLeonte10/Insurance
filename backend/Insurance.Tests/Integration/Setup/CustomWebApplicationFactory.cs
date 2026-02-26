
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
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
    private SqliteConnection _reportingConnection = null!;
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

            var insuranceDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<InsuranceDbContext>))
                .ToList();

            foreach (var descriptor in insuranceDescriptors)
                services.Remove(descriptor);

            var reportingDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<ReportingDbContext>))
                .ToList();

            foreach (var reportingDescriptor in reportingDescriptors)
                services.Remove(reportingDescriptor);

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<InsuranceDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            _reportingConnection = new SqliteConnection("DataSource=:memory:");
            _reportingConnection.Open();

            services.AddDbContext <ReportingDbContext>(options =>
            {
                options.UseSqlite(_reportingConnection);
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
