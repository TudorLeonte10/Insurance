using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using Insurance.Tests.Integration.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Insurance.Tests.Integration
{
    public class PolicyIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task FullFlow_CreateClientBuildingPolicy_AndActivatePolicy_ShouldSucced()
        {
            var (factory, client) = CreateTestContext();

            var brokerId = await CreateBrokerAndGetId(client);

            SetRoles(client, "Broker,Admin");
            client.DefaultRequestHeaders.Remove("X-Test-BrokerId");
            client.DefaultRequestHeaders.Add("X-Test-BrokerId", brokerId.ToString());

            var clientId = await CreateClientAndGetId(client);
            var cityId = factory.SeededCityId;
            var buildingId = await CreateBuildingAndGetId(client, cityId, clientId);

            var policyDto = new
            {
                ClientId = clientId,
                BuildingId = buildingId,
                BrokerId = brokerId,
                CurrencyId = factory.SeededCurrencyId,
                BasePremium = 100,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddYears(1)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/brokers/policies",
                policyDto);

            createResponse.EnsureSuccessStatusCode();

            var policyId = Guid.Parse(
                createResponse.Headers.Location!.Segments.Last());

            var activateResponse = await client.PostAsync(
                $"/api/brokers/policies/{policyId}/activate",
                null);

            activateResponse.EnsureSuccessStatusCode();

            var getResponse = await client.GetAsync(
                $"/api/brokers/policies/{policyId}");

            getResponse.EnsureSuccessStatusCode();

            var rawJson = await getResponse.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(rawJson);
            var root = doc.RootElement;

            Assert.Equal(policyId, root.GetProperty("id").GetGuid());
            Assert.Equal(clientId, root.GetProperty("client").GetProperty("id").GetGuid());

            Assert.Equal(buildingId, root.GetProperty("building").GetProperty("id").GetGuid());


            Assert.Equal("active", root.GetProperty("status").GetString());
        }
        [Fact]
        public async Task CreatePolicy_WithNonExistentClient_ShouldReturnBadRequest()
        {
            var (factory, client) = CreateTestContext();

            var brokerId = await CreateBrokerAndGetId(client);

            var dto = new
            {
                ClientId = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                BrokerId = brokerId,
                CurrencyId = factory.SeededCurrencyId,
                BasePremium = 100,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddYears(1)
            };

            var response = await client.PostAsJsonAsync(
                "/api/brokers/policies",
                dto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task CreatePolicy_WithNegativePremium_ShouldReturnBadRequest()
        {
            var (factory, client) = CreateTestContext();

            var brokerId = await CreateBrokerAndGetId(client);
            var clientId = await CreateClientAndGetId(client);
            var cityId = factory.SeededCityId;
            var buildingId = await CreateBuildingAndGetId(client, cityId, clientId);

            var dto = new
            {
                ClientId = clientId,
                BuildingId = buildingId,
                BrokerId = brokerId,
                CurrencyId = factory.SeededCurrencyId,
                BasePremium = -10,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddYears(1)
            };

            var response = await client.PostAsJsonAsync(
                "/api/brokers/policies",
                dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreatePolicy_WithDeactivatedBroker_ShouldFail()
        {
            var (factory, client) = CreateTestContext();

            var brokerId = await CreateBrokerAndGetId(client);

            SetRoles(client, "Broker,Admin");
            client.DefaultRequestHeaders.Remove("X-Test-BrokerId");
            client.DefaultRequestHeaders.Add("X-Test-BrokerId", brokerId.ToString());

            var clientId = await CreateClientAndGetId(client);
            var cityId = factory.SeededCityId;
            var buildingId = await CreateBuildingAndGetId(client, cityId, clientId);

            var deactivateResponse = await client.PatchAsync(
                $"/api/admin/brokers/{brokerId}/deactivate",
                null);

            deactivateResponse.EnsureSuccessStatusCode();

            var dto = new
            {
                ClientId = clientId,
                BuildingId = buildingId,
                BrokerId = brokerId,
                CurrencyId = factory.SeededCurrencyId,
                BasePremium = 100,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddYears(1)
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/brokers/policies",
                dto);

            Assert.True(
                createResponse.StatusCode == HttpStatusCode.BadRequest ||
                createResponse.StatusCode == HttpStatusCode.Forbidden);
        }

    }
}
