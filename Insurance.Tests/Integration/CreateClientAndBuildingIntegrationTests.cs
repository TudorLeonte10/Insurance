using Insurance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.Tests.Integration
{
    public class CreateClientAndBuildingIntegrationTests
    {
        private (CustomWebApplicationFactory factory, HttpClient client) CreateTestContext()
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

        [Fact]
        public async Task CreateClient_Then_CreateBuilding_ShouldSucceed()
        {
            var (factory, client) = CreateTestContext();

            var createClientDto = new
            {
                Name = "Calin",
                IdentificationNumber = DateTime.UtcNow.Ticks.ToString(),
                Email = "calin@gmail.com",
                PhoneNumber = "0712345678",
                Address = "Strada Libertatii 15",
                Type = 1
            };

            var clientResponse =
                await client.PostAsJsonAsync("/api/brokers/clients", createClientDto);

            Assert.Equal(HttpStatusCode.Created, clientResponse.StatusCode);

            var clientId = Guid.Parse(
                clientResponse.Headers.Location!.Segments.Last());

            var createBuildingDto = new
            {
                Street = "Strada Memorandumului",
                Number = "10",
                CityId = factory.SeededCityId,
                ConstructionYear = 2000,
                Type = 1,
                SurfaceArea = 120,
                NumberOfFloors = 6,
                InsuredValue = 150000,
                RiskIndicators = new[] { 1 }
            };

            var buildingResponse =
                await client.PostAsJsonAsync(
                    $"/api/brokers/clients/{clientId}/buildings",
                    createBuildingDto);

            Assert.Equal(HttpStatusCode.Created, buildingResponse.StatusCode);
            Assert.NotNull(buildingResponse.Headers.Location);
        }

        [Fact]
        public async Task CreateClient_WithDuplicateIdentificationNumber_ShouldReturn409()
        {
            var (_, client) = CreateTestContext();

            var dto = new
            {
                Name = "Ion Popescu",
                IdentificationNumber = "9999999999",
                Email = "ion@test.ro",
                PhoneNumber = "0711111111",
                Address = "Strada Libertatii 16",
                Type = 1
            };

            var first = await client.PostAsJsonAsync("/api/brokers/clients", dto);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            var second = await client.PostAsJsonAsync("/api/brokers/clients", dto);
            Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        }

        [Fact]
        public async Task CreateBuilding_WithInvalidCityId_ShouldReturn404()
        {
            var (_, client) = CreateTestContext();
            var clientId = await CreateClientAndGetId(client);

            var dto = new
            {
                Street = "Test",
                Number = "1",
                CityId = Guid.NewGuid(), 
                ConstructionYear = 2000,
                Type = 1,
                SurfaceArea = 100,
                NumberOfFloors = 1,
                InsuredValue = 100000,
                RiskIndicators = new[] { 1 }
            };

            var response =
                await client.PostAsJsonAsync(
                    $"/api/brokers/clients/{clientId}/buildings", dto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBuilding_ForMissingClient_ShouldReturn404()
        {
            var (factory, client) = CreateTestContext();

            var dto = new
            {
                Street = "Test",
                Number = "1",
                CityId = factory.SeededCityId,
                ConstructionYear = 2000,
                Type = 1,
                SurfaceArea = 100,
                NumberOfFloors = 1,
                InsuredValue = 100000,
                RiskIndicators = new[] { 1 }
            };

            var response =
                await client.PostAsJsonAsync(
                    $"/api/brokers/clients/{Guid.NewGuid()}/buildings", dto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private static async Task<Guid> CreateClientAndGetId(HttpClient client)
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

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var location = response.Headers.Location;
            Assert.NotNull(location);

            return Guid.Parse(response.Headers.Location!.Segments.Last());
        }
    }
}
