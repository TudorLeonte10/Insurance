using Insurance.Application.Common.Paging;
using Insurance.Application.FeeConfiguration.DTOs;
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
    public class FeeConfigurationsIntegrationTests
    : IntegrationTestBase
    {
        [Fact]
        public async Task POST_Fees_Should_Create_FeeConfiguration()
        { 
            var (_, client) = CreateTestContext();

            var dto = new
            {
                name = "Admin fee",
                type = "AdminFee",
                percentage = 0.05,
                effectiveFrom = DateTime.Today,
                effectiveTo = DateTime.Today.AddDays(30),
                isActive = true
            };

            var response = await client.PostAsJsonAsync(
                "/api/admin/fees", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task GET_Fees_Should_Return_List()
        {
            var (_, client) = CreateTestContext();

            await client.PostAsJsonAsync("/api/admin/fees", new
            {
                name = "Broker commission",
                type = "BrokerCommission",
                percentage = 0.1,
                effectiveFrom = DateTime.Today,
                isActive = true
            });

            var response = await client.GetAsync("/api/admin/fees");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            var result = await response.Content
                .ReadFromJsonAsync<PagedResult<FeeConfigurationDto>>(jsonOptions);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }


        [Fact]
        public async Task PUT_Fees_Should_Update_FeeConfiguration()
        {
            var (_, client) = CreateTestContext();

            var createResponse = await client.PostAsJsonAsync(
                "/api/admin/fees",
                new
                {
                    name = "Admin fee",
                    type = "AdminFee",
                    percentage = 0.02,
                    effectiveFrom = DateTime.Today,
                    isActive = true
                });

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);


            var getResponse = await client.GetAsync("/api/admin/fees");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.Converters.Add(new JsonStringEnumConverter());

            var list = await getResponse.Content
                .ReadFromJsonAsync<PagedResult<FeeConfigurationDto>>(options);

            Assert.NotNull(list);
            Assert.NotEmpty(list.Items);

            var id = list.Items.First().Id;

            var updateDto = new
            {
                name = "Admin fee updated",
                type = "AdminFee",
                percentage = 0.15,
                effectiveFrom = DateTime.Today,
                effectiveTo = DateTime.Today.AddDays(60),
                isActive = false
            };

            var updateResponse = await client.PutAsJsonAsync(
                $"/api/admin/fees/{id}",
                updateDto);

            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        }


        [Fact]
        public async Task PUT_Fees_Should_Return_404_When_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var updateDto = new
            {
                name = "Not found fee",
                type = "AdminFee",
                percentage = 0.05,
                effectiveFrom = DateTime.Today,
                isActive = true
            };

            var response = await client.PutAsJsonAsync(
                $"/api/admin/fees/{Guid.NewGuid()}",
                updateDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

}

