using Insurance.Tests.Integration.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Insurance.Tests.Integration
{
    public class BrokersE2EIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task BrokerFullFlow_CreateUpdateDeactivateActivateGetAll_ShouldSucceed()
        {
            var (factory, client) = CreateTestContext();

            var createDto = new
            {
                BrokerCode = $"BR-{Guid.NewGuid().ToString()[..8]}",
                Name = "Test Broker E2E",
                Email = "broker.e2e@test.com",
                Phone = "0712345678"
            };

            var createResponse = await client.PostAsJsonAsync(
                "/api/admin/brokers",
                createDto);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            Assert.NotNull(createResponse.Headers.Location);

            var brokerId = Guid.Parse(
                createResponse.Headers.Location.Segments[^1]);

            var getByIdResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            Assert.Equal(HttpStatusCode.OK, getByIdResponse.StatusCode);

            var brokerJson = await getByIdResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(brokerJson);
            var root = doc.RootElement;

            Assert.Equal(brokerId, root.GetProperty("id").GetGuid());
            Assert.Equal(createDto.BrokerCode, root.GetProperty("brokerCode").GetString());
            Assert.Equal(createDto.Name, root.GetProperty("name").GetString());
            Assert.Equal(createDto.Email, root.GetProperty("email").GetString());
            Assert.True(root.GetProperty("isActive").GetBoolean());

            var updateDto = new
            {
                BrokerCode = createDto.BrokerCode,
                Name = "Updated Broker Name",
                Email = "updated.broker@test.com",
                Phone = "0798765432"
            };

            var updateResponse = await client.PutAsJsonAsync(
                $"/api/admin/brokers/{brokerId}",
                updateDto);

            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            var verifyUpdateResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var updatedJson = await verifyUpdateResponse.Content.ReadAsStringAsync();
            using var updatedDoc = JsonDocument.Parse(updatedJson);
            var updatedRoot = updatedDoc.RootElement;

            Assert.Equal("Updated Broker Name", updatedRoot.GetProperty("name").GetString());
            Assert.Equal("updated.broker@test.com", updatedRoot.GetProperty("email").GetString());

     
            var deactivateResponse = await client.PatchAsync(
                $"/api/admin/brokers/{brokerId}/deactivate",
                null);

            Assert.Equal(HttpStatusCode.NoContent, deactivateResponse.StatusCode);

         
            var verifyDeactivateResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var deactivatedJson = await verifyDeactivateResponse.Content.ReadAsStringAsync();
            using var deactivatedDoc = JsonDocument.Parse(deactivatedJson);
            var deactivatedRoot = deactivatedDoc.RootElement;

            Assert.False(deactivatedRoot.GetProperty("isActive").GetBoolean());

          
            var activateResponse = await client.PatchAsync(
                $"/api/admin/brokers/{brokerId}/activate",
                null);

            Assert.Equal(HttpStatusCode.NoContent, activateResponse.StatusCode);

            var verifyActivateResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var activatedJson = await verifyActivateResponse.Content.ReadAsStringAsync();
            using var activatedDoc = JsonDocument.Parse(activatedJson);
            var activatedRoot = activatedDoc.RootElement;

            Assert.True(activatedRoot.GetProperty("isActive").GetBoolean());

            var getAllResponse = await client.GetAsync(
                "/api/admin/brokers?pageNumber=1&pageSize=10");

            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

            var allBrokersJson = await getAllResponse.Content.ReadAsStringAsync();
            using var allBrokersDoc = JsonDocument.Parse(allBrokersJson);
            var allBrokersRoot = allBrokersDoc.RootElement;

            Assert.True(allBrokersRoot.GetProperty("items").GetArrayLength() > 0);
            Assert.True(allBrokersRoot.GetProperty("totalCount").GetInt32() > 0);
        }

        [Fact]
        public async Task GetBrokerById_WithNonExistentId_ShouldReturn404()
        {
            var (factory, client) = CreateTestContext();
            var nonExistentId = Guid.NewGuid();

            var response = await client.GetAsync(
                $"/api/admin/brokers/{nonExistentId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBroker_WithInvalidData_ShouldReturn400()
        {
            var (factory, client) = CreateTestContext();

            var invalidDto = new
            {
                BrokerCode = "", 
                Name = "", 
                Email = "invalid-email", 
                Phone = ""
            };

            var response = await client.PostAsJsonAsync(
                "/api/admin/brokers",
                invalidDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBroker_WithNonExistentId_ShouldReturn404()
        {
            var (factory, client) = CreateTestContext();
            var nonExistentId = Guid.NewGuid();

            var updateDto = new
            {
                BrokerCode = "BR-TEST",
                Name = "Test",
                Email = "test@test.com",
                Phone = "0712345678"
            };

            var response = await client.PutAsJsonAsync(
                $"/api/admin/brokers/{nonExistentId}",
                updateDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBrokers_WithPagination_ShouldReturnPagedResults()
        {
            var (factory, client) = CreateTestContext();

            for (int i = 0; i < 3; i++)
            {
                var createDto = new
                {
                    BrokerCode = $"BR-PAGE-{i}",
                    Name = $"Broker {i}",
                    Email = $"broker{i}@test.com",
                    Phone = "0712345678"
                };

                await client.PostAsJsonAsync("/api/admin/brokers", createDto);
            }

            var response = await client.GetAsync(
                "/api/admin/brokers?pageNumber=1&pageSize=2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var items = root.GetProperty("items");
            var pageNumber = root.GetProperty("pageNumber").GetInt32();
            var pageSize = root.GetProperty("pageSize").GetInt32();
            var totalCount = root.GetProperty("totalCount").GetInt32();

            Assert.True(items.GetArrayLength() <= 2);
            Assert.Equal(1, pageNumber);
            Assert.Equal(2, pageSize);
            Assert.True(totalCount >= 3);
        }
    }
}
