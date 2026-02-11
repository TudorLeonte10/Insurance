using Insurance.Application.Geography.DTOs;
using Insurance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.Tests.Integration.Geography
{
    public class GeographyControllerIntegrationTests
    {
        [Fact]
        public async Task GetCountries_Should_ReturnSeededCountries()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
                db.Database.EnsureCreated();
                factory.SeedTestData(db);
            }

            var response = await client.GetAsync("/api/brokers/countries");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();
            Assert.NotEmpty(countries!);
        }

        [Fact]
        public async Task GetCounties_Should_ReturnCountiesForCountry()
        {
            var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            Guid countryId;

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
                db.Database.EnsureCreated();
                factory.SeedTestData(db);
                countryId = db.Countries.First().Id;
            }

            var response = await client.GetAsync($"/api/brokers/countries/{countryId}/counties");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
