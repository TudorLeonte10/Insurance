using Insurance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Json;

namespace Insurance.Tests.Integration.Setup;

public abstract class IntegrationTestBase
{
    protected (CustomWebApplicationFactory factory, HttpClient client) CreateTestContext()
    {
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            db.Database.EnsureCreated();
            factory.SeedTestData(db);
        }

        return (factory, client);
    }

    protected async Task<Guid> CreateClientAndGetId(HttpClient client)
    {
        var dto = new
        {
            Name = "Helper Client",
            IdentificationNumber = DateTime.UtcNow.Ticks.ToString(),
            Email = "helper@test.ro",
            PhoneNumber = "0700000000",
            Address = "Strada Nicolina 5",
            Type = 1
        };

        var response =
            await client.PostAsJsonAsync("/api/brokers/clients", dto);

        response.EnsureSuccessStatusCode();

        return Guid.Parse(
            response.Headers.Location!.Segments.Last());
    }

    protected async Task<Guid> CreateBuildingAndGetId(
        HttpClient client,
        Guid cityId,
        Guid clientId)
    {
        var dto = new
        {
            Street = "Test Street",
            Number = "10",
            CityId = cityId,
            ConstructionYear = 2000,
            Type = 1,
            SurfaceArea = 120,
            NumberOfFloors = 2,
            InsuredValue = 150000,
            RiskIndicators = new[] { 1 }
        };

        var response =
            await client.PostAsJsonAsync(
                $"/api/brokers/clients/{clientId}/buildings", dto);

        response.EnsureSuccessStatusCode();

        return Guid.Parse(
            response.Headers.Location!.Segments.Last());
    }

    protected async Task<(Guid clientId, string identificationNumber)>
    CreateClientAndGetIdWithIdentification(HttpClient client)
    {
        var identificationNumber = DateTime.UtcNow.Ticks.ToString();

        var dto = new
        {
            Name = "Helper Client",
            IdentificationNumber = identificationNumber,
            Email = "helper@test.ro",
            PhoneNumber = "0700000000",
            Address = "Strada Nicolina 5",
            Type = 1
        };

        var response =
            await client.PostAsJsonAsync("/api/brokers/clients", dto);

        response.EnsureSuccessStatusCode();

        var clientId = Guid.Parse(
            response.Headers.Location!.Segments.Last());

        return (clientId, identificationNumber);
    }

}
