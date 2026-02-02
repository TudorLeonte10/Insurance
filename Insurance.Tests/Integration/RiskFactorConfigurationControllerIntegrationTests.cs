using Azure;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Tests.Integration.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Tests.Integration
{
    public class RiskFactorsIntegrationTests
    : IntegrationTestBase
    {
        [Fact]
        public async Task POST_RiskFactors_Should_Create_RiskFactor()
        {
            var (_, client) = CreateTestContext();

            var dto = new
            {
                level = 1,
                referenceId = "CITY_FLOOD",
                adjustmentPercentage = 15
            };

            var response = await client.PostAsJsonAsync(
                "/api/admin/risk-factors", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }
        

        [Fact]
        public async Task GET_RiskFactors_Should_Return_List()
        {
            var (_, client) = CreateTestContext();

            await client.PostAsJsonAsync("/api/admin/risk-factors", new
            {
                level = 1,
                referenceId = "CITY_FIRE",
                adjustmentPercentage = 10
            });

            var response = await client.GetAsync("/api/admin/risk-factors");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<PagedResult<RiskFactorConfigurationDto>>();


            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task PUT_RiskFactors_Should_Update_RiskFactor()
        {
            var (_, client) = CreateTestContext();

            var createResponse = await client.PostAsJsonAsync(
                "/api/admin/risk-factors",
                new
                {
                    level = 1,
                    referenceId = "CITY_QUAKE",
                    adjustmentPercentage = 5
                });

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var location = createResponse.Headers.Location!.ToString();

            var id = Guid.Parse(
                System.Web.HttpUtility.ParseQueryString(
                    new Uri("http://localhost" + location).Query
                )["id"]!);

            var updateDto = new
            {
                level = 2,
                referenceId = "CITY_QUAKE_UPDATED",
                adjustmentPercentage = 20
            };

            var updateResponse = await client.PutAsJsonAsync(
                $"/api/admin/risk-factors/{id}",
                updateDto);

            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        }



        [Fact]
        public async Task PUT_RiskFactors_Should_Return_404_When_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var updateDto = new
            {
                level = 1,
                referenceId = "NOT_FOUND",
                adjustmentPercentage = 10
            };

            var response = await client.PutAsJsonAsync(
                $"/api/admin/risk-factors/{Guid.NewGuid()}",
                updateDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }

}
