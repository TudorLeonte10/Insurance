using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Common.Paging;
using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence;
using Insurance.Tests.Integration.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Tests.Integration
{
    public class BrokersControllerIntegrationTests
    : IntegrationTestBase, IClassFixture<ControllerTestWebApplicationFactory>
    {

        private readonly HttpClient _client;
        private readonly ControllerTestWebApplicationFactory _factory;

        public BrokersControllerIntegrationTests(ControllerTestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task POST_Admin_Brokers_Should_Return_201()
        {
            var payload = new
            {
                brokerCode = "BR001",
                name = "Test Broker",
                email = "test@test.com",
                phone = "123"
            };

            var response = await _client.PostAsJsonAsync(
                "/api/admin/brokers", payload);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GET_Admin_Brokers_Should_Return_Paged_Result()
        {
            var (factory, client) = CreateTestContext();

            await CreateBrokerAndGetId(client);

            var response =
                await client.GetAsync("/api/admin/brokers?pageNumber=1&pageSize=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<PagedResult<BrokerDetailsDto>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task GET_Admin_Broker_By_Id_Should_Return_Broker()
        {
            var (factory, client) = CreateTestContext();

            var brokerId = await CreateBrokerAndGetId(client);

            var response =
                await client.GetAsync($"/api/admin/brokers/{brokerId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var broker = await response.Content
                .ReadFromJsonAsync<BrokerDetailsDto>();

            Assert.NotNull(broker);
            Assert.Equal(brokerId, broker.Id);
        }

        [Fact]
        public async Task GET_Admin_Broker_By_Id_Should_Return_404_When_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var response =
                await client.GetAsync($"/api/admin/brokers/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task PATCH_Activate_Broker_Should_Set_IsActive_To_True()
        {
            var (factory, client) = CreateTestContext();
            var brokerId = await CreateBrokerAndGetId(client);

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<InsuranceDbContext>();

                var broker = await db.Brokers.FindAsync(brokerId);
                broker!.IsActive = false;
                await db.SaveChangesAsync();
            }

            var response = await client.PatchAsync(
                $"/api/admin/brokers/{brokerId}/activate",
                null);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var brokerDto = await getResponse.Content
                .ReadFromJsonAsync<BrokerDetailsDto>();

            Assert.True(brokerDto!.IsActive);
        }

        [Fact]
        public async Task PATCH_Deactivate_Broker_Should_Set_IsActive_To_False()
        {
            var (_, client) = CreateTestContext();
            var brokerId = await CreateBrokerAndGetId(client);

            var response = await client.PatchAsync(
                $"/api/admin/brokers/{brokerId}/deactivate",
                null);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var brokerDto = await getResponse.Content
                .ReadFromJsonAsync<BrokerDetailsDto>();

            Assert.False(brokerDto!.IsActive);
        }

        [Fact]
        public async Task PATCH_Activate_Should_Return_404_When_Broker_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var response = await client.PatchAsync(
                $"/api/admin/brokers/{Guid.NewGuid()}/activate",
                null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PATCH_Deactivate_Should_Return_404_When_Broker_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var response = await client.PatchAsync(
                $"/api/admin/brokers/{Guid.NewGuid()}/deactivate",
                null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Admin_Broker_Should_Update_Details()
        {
            var (factory, client) = CreateTestContext();
            var brokerId = await CreateBrokerAndGetId(client);

            var updateDto = new
            {
                name = "Updated Broker",
                email = "updated@test.com",
                phone = "0711111111"
            };

            var response = await client.PutAsJsonAsync(
                $"/api/admin/brokers/{brokerId}",
                updateDto);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getResponse = await client.GetAsync(
                $"/api/admin/brokers/{brokerId}");

            var broker = await getResponse.Content
                .ReadFromJsonAsync<BrokerDetailsDto>();

            Assert.Equal("Updated Broker", broker!.Name);
            Assert.Equal("updated@test.com", broker.Email);
            Assert.Equal("0711111111", broker.Phone);
        }

        [Fact]
        public async Task PUT_Admin_Broker_Should_Return_404_When_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var updateDto = new
            {
                name = "Updated Broker",
                email = "updated@test.com",
                phone = "0711111111"
            };

            var response = await client.PutAsJsonAsync(
                $"/api/admin/brokers/{Guid.NewGuid()}",
                updateDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}



