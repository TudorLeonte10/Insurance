using Insurance.Infrastructure.Persistence;
using Insurance.Tests.Integration.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.Tests.Integration
{
    public class CreateClientAndBuildingIntegrationTests : IntegrationTestBase
    {
        
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
    }
}
