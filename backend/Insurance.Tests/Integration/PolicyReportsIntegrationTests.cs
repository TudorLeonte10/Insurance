using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
using Insurance.Tests.Integration.Setup;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace Insurance.Tests.Integration
{
    public class PolicyReportsIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task PoliciesByCity_AggregationsMatchSeededData()
        {
            var (factory, client) = CreateTestContext();

            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();
                db.Database.EnsureCreated();

                db.PolicyReportAggregates.AddRange(
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BrokerCode = "BR1",
                        Currency = "RON",
                        Status = "Active",
                        BuildingType = "Residential",
                        FinalPremium = 100m,
                        FinalPremiumInBase = 100m,
                        StartDate = DateTime.UtcNow.AddDays(-10),
                        EndDate = DateTime.UtcNow.AddDays(10),
                        CreatedAt = DateTime.UtcNow
                    },
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BrokerCode = "BR2",
                        Currency = "RON",
                        Status = "Active",
                        BuildingType = "Residential",
                        FinalPremium = 200m,
                        FinalPremiumInBase = 200m,
                        StartDate = DateTime.UtcNow.AddDays(-5),
                        EndDate = DateTime.UtcNow.AddDays(5),
                        CreatedAt = DateTime.UtcNow
                    },
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Bucharest",
                        City = "Bucharest",
                        BrokerCode = "BR1",
                        Currency = "EUR",
                        Status = "Active",
                        BuildingType = "Commercial",
                        FinalPremium = 150m,
                        FinalPremiumInBase = 330m,
                        StartDate = DateTime.UtcNow.AddDays(-5),
                        EndDate = DateTime.UtcNow.AddDays(5),
                        CreatedAt = DateTime.UtcNow
                    }
                );

                await db.SaveChangesAsync();
            }

            var from = DateTime.UtcNow.AddHours(-1).ToString("o");
            var to = DateTime.UtcNow.AddHours(1).ToString("o");

            // new controller endpoint: /api/admin/policies/reports
            var response = await client.GetAsync($"/api/admin/policies/reports?reportGroupingType=City&from={Uri.EscapeDataString(from)}&to={Uri.EscapeDataString(to)}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            Assert.True(root.ValueKind == JsonValueKind.Array);

            var items = root.EnumerateArray().ToArray();
            Assert.Equal(2, items.Length);

            var clujRon = items.Single(i => i.GetProperty("groupName").GetString() == "Cluj-Napoca" && i.GetProperty("currency").GetString() == "RON");
            Assert.Equal(2, clujRon.GetProperty("policiesCount").GetInt32());
            Assert.Equal(300m, clujRon.GetProperty("totalPremium").GetDecimal());

            var bucharestEur = items.Single(i => i.GetProperty("groupName").GetString() == "Bucharest" && i.GetProperty("currency").GetString() == "EUR");
            Assert.Equal(1, bucharestEur.GetProperty("policiesCount").GetInt32());
            Assert.Equal(150m, bucharestEur.GetProperty("totalPremium").GetDecimal());
        }

        [Fact]
        public async Task PoliciesByBroker_FiltersChangeResultsAsExpected()
        {
            var (factory, client) = CreateTestContext();

            var now = DateTime.UtcNow;
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();
                db.Database.EnsureCreated();

                db.PolicyReportAggregates.AddRange(
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BrokerCode = "BR1",
                        Currency = "RON",
                        Status = "Active",
                        BuildingType = "Residential",
                        FinalPremium = 100m,
                        FinalPremiumInBase = 100m,
                        StartDate = now.AddDays(-5),
                        EndDate = now.AddDays(5),
                        CreatedAt = now.AddDays(-2)
                    },
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Cluj",
                        City = "Cluj-Napoca",
                        BrokerCode = "BR1",
                        Currency = "EUR",
                        Status = "Cancelled",
                        BuildingType = "Residential",
                        FinalPremium = 200m,
                        FinalPremiumInBase = 440m,
                        StartDate = now.AddDays(-5),
                        EndDate = now.AddDays(5),
                        CreatedAt = now.AddDays(-1)
                    },
                    new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = "RO",
                        County = "Bucharest",
                        City = "Bucharest",
                        BrokerCode = "BR2",
                        Currency = "RON",
                        Status = "Active",
                        BuildingType = "Commercial",
                        FinalPremium = 300m,
                        FinalPremiumInBase = 300m,
                        StartDate = now.AddDays(-5),
                        EndDate = now.AddDays(5),
                        CreatedAt = now
                    }
                );

                await db.SaveChangesAsync();
            }

            var from = Uri.EscapeDataString(now.AddDays(-10).ToString("o"));
            var to = Uri.EscapeDataString(now.AddDays(1).ToString("o"));

            // first: all group by Broker
            var respAll = await client.GetAsync($"/api/admin/policies/reports?reportGroupingType=Broker&from={from}&to={to}");
            Assert.Equal(HttpStatusCode.OK, respAll.StatusCode);
            var allJson = await respAll.Content.ReadAsStringAsync();
            using var allDoc = JsonDocument.Parse(allJson);
            var allItems = allDoc.RootElement.EnumerateArray().ToArray();
            Assert.True(allItems.Length >= 2);

            // filter by status=Active
            var respActive = await client.GetAsync($"/api/admin/policies/reports?reportGroupingType=Broker&from={from}&to={to}&status=Active");
            Assert.Equal(HttpStatusCode.OK, respActive.StatusCode);
            var activeJson = await respActive.Content.ReadAsStringAsync();
            using var activeDoc = JsonDocument.Parse(activeJson);
            var activeItems = activeDoc.RootElement.EnumerateArray().ToArray();

            var br1Ron = activeItems.Single(i => i.GetProperty("groupName").GetString() == "BR1" && i.GetProperty("currency").GetString() == "RON");
            Assert.Equal(1, br1Ron.GetProperty("policiesCount").GetInt32());
            Assert.Equal(100m, br1Ron.GetProperty("totalPremium").GetDecimal());

            // filter by currency=RON
            var respRon = await client.GetAsync($"/api/admin/policies/reports?reportGroupingType=Broker&from={from}&to={to}&currency=RON");
            Assert.Equal(HttpStatusCode.OK, respRon.StatusCode);

            var ronJson = await respRon.Content.ReadAsStringAsync();
            using var ronDoc = JsonDocument.Parse(ronJson);
            var ronItems = ronDoc.RootElement.EnumerateArray().ToArray();

            var br1 = ronItems.Single(i => i.GetProperty("groupName").GetString() == "BR1");
            Assert.Equal("RON", br1.GetProperty("currency").GetString());
        }
    }
}