using Insurance.Application.Geography.DTOs;
using Insurance.Infrastructure.Persistence;
using Insurance.Tests.Integration.Setup;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.Tests.Integration.Geography
{
    public class GeographyControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetCountries_Should_ReturnSeededCountries()
        {
            var (factory, client) = CreateTestContext(); 

            var response = await client.GetAsync("/api/brokers/countries");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();
            Assert.NotEmpty(countries!);
        }

        [Fact]
        public async Task GetCounties_Should_ReturnCountiesForCountry()
        {
            var (factory, client) = CreateTestContext(); // <-- uses test auth + seed

            var countryId = factory.SeededCityId == Guid.Empty
                ? throw new InvalidOperationException("Factory did not seed geography.")
                : await GetSeededCountryId(factory);

            var response = await client.GetAsync($"/api/brokers/countries/{countryId}/counties");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        private static async Task<Guid> GetSeededCountryId(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            db.Database.EnsureCreated();
           
            return db.Countries.First().Id;
        }
    }
}
