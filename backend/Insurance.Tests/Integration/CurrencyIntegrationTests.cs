using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Tests.Integration.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Tests.Integration
{
    public class CurrencyIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task POST_Currencies_Should_Create_Currency()
        {
            var (_, client) = CreateTestContext();

            var response = await client.PostAsJsonAsync(
                "/api/admin/currencies",
                new
                {
                    code = "EUR",
                    name = "Euro",
                    exchangeRateToBase = 4.95
                });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GET_Currencies_Should_Return_List()
        {
            var (_, client) = CreateTestContext();

            await client.PostAsJsonAsync("/api/admin/currencies", new
            {
                code = "USD",
                name = "US Dollar",
                exchangeRateToBase = 4.6
            });

            var response = await client.GetAsync("/api/admin/currencies");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<PagedResult<CurrencyDto>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task PUT_Currencies_Should_Update_Currency()
        {
            var (_, client) = CreateTestContext();

            // CREATE
            await client.PostAsJsonAsync("/api/admin/currencies", new
            {
                code = "RON",
                name = "Romanian Leu",
                exchangeRateToBase = 1
            });

            var getResponse = await client.GetAsync("/api/admin/currencies");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var list = await getResponse.Content
                .ReadFromJsonAsync<PagedResult<CurrencyDto>>();

            Assert.NotNull(list);
            Assert.NotEmpty(list.Items);

            var id = list.Items.First().Id;

            var updateResponse = await client.PutAsJsonAsync(
                $"/api/admin/currencies/{id}",
                new
                {
                    code = "RON",
                    name = "Romanian Leu Updated",
                    exchangeRateToBase = 1.05
                });

            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        }

        [Fact]
        public async Task PUT_Currencies_Should_Return_404_When_Not_Found()
        {
            var (_, client) = CreateTestContext();

            var response = await client.PutAsJsonAsync(
                $"/api/admin/currencies/{Guid.NewGuid()}",
                new
                {
                    code = "GBP",
                    name = "British Pound",
                    exchangeRateToBase = 5.7
                });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

}

