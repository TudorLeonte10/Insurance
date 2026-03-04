using Insurance.Tests.Integration;
using Insurance.Tests.Integration.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Insurance.Tests.Integration
{
    public class BuildingsControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetBuildingsByClient_Should_ReturnBuildings()
        {
            var (factory, client) = CreateTestContext();

            var clientId = await CreateClientAndGetId(client);

            var response = await client.GetAsync(
                $"/api/brokers/clients/{clientId}/buildings");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBuildingById_Should_ReturnBuilding()
        {
            var (factory, client) = CreateTestContext();

            var clientId = await CreateClientAndGetId(client);

            var buildingId = await CreateBuildingAndGetId(client, factory.SeededCityId, clientId);

            var response = await client.GetAsync(
                $"/api/brokers/buildings/{buildingId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
