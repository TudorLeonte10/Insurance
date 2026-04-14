using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class PolicyReportAggregateSeeder
    {
        private readonly InsuranceDbContext _insuranceDb;
        private readonly ReportingDbContext _reportingDb;
        private static readonly Random _rng = new(42);

        private static readonly (string Country, string County, string City)[] PrimaryLocations =
        {
            ("Romania", "Iasi",   "Iasi"),
            ("Romania", "Iasi",   "Pascani"),
            ("Romania", "Cluj",   "Cluj-Napoca"),
            ("Romania", "Cluj",   "Dej"),
            ("Romania", "Cluj",   "Turda"),
            ("Romania", "Ilfov",  "Sector 1"),
            ("Romania", "Ilfov",  "Sector 3"),
            ("Romania", "Ilfov",  "Sector 6"),
        };

        private static readonly (string Country, string County, string City)[] SecondaryLocations =
        {
            ("Romania", "Timis",    "Timisoara"),
            ("Romania", "Constanta","Constanta"),
            ("Romania", "Brasov",   "Brasov"),
            ("Romania", "Dolj",     "Craiova"),
            ("Romania", "Bihor",    "Oradea"),
            ("Romania", "Sibiu",    "Sibiu"),
            ("Romania", "Bacau",    "Bacau"),
            ("Romania", "Prahova",  "Ploiesti"),
            ("Romania", "Arges",    "Pitesti"),
            ("Romania", "Arad",     "Arad"),
            ("Romania", "Mures",    "Targu Mures"),
            ("Romania", "Galati",   "Galati"),
        };

        private static readonly string[] BuildingTypes = { "House", "Apartment", "Commercial", "Industrial" };

        private static readonly (string Status, int Weight)[] StatusWeights =
        {
            ("Active",     55),
            ("Expired",    15),
            ("Cancelled",  12),
            ("Draft",      11),
            ("Rejected",    7),
        };

        public PolicyReportAggregateSeeder(InsuranceDbContext insuranceDb, ReportingDbContext reportingDb)
        {
            _insuranceDb = insuranceDb;
            _reportingDb = reportingDb;
        }

        public async Task SeedAsync()
        {
            if (_reportingDb.PolicyReportAggregates.Any())
                return;

            var brokerCodes = await _insuranceDb.Brokers
                .AsNoTracking()
                .Select(b => b.BrokerCode)
                .ToListAsync();

            var currencies = await _insuranceDb.Currencies
                .AsNoTracking()
                .ToListAsync();

            var aggregates = new List<PolicyReportAggregate>(3100);
            var now = DateTime.UtcNow;

            for (int monthsBack = 23; monthsBack >= 0; monthsBack--)
            {
                var monthBase = now.AddMonths(-monthsBack);
                var count = 90 + (23 - monthsBack) * 3 + _rng.Next(-6, 10);

                for (int i = 0; i < count; i++)
                {
                    var dayOffset = _rng.Next(0, DateTime.DaysInMonth(monthBase.Year, monthBase.Month));
                    var startDate = new DateTime(monthBase.Year, monthBase.Month, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddDays(dayOffset);
                    var createdAt = startDate.AddDays(_rng.Next(-5, 3));
                    var endDate = startDate.AddYears(1);

                    var usePrimary = _rng.NextDouble() < 0.77;
                    var loc = usePrimary
                        ? PrimaryLocations[_rng.Next(PrimaryLocations.Length)]
                        : SecondaryLocations[_rng.Next(SecondaryLocations.Length)];

                    var buildingType = BuildingTypes[_rng.Next(BuildingTypes.Length)];
                    var brokerCode = brokerCodes[_rng.Next(brokerCodes.Count)];
                    var currency = currencies[_rng.Next(currencies.Count)];
                    var status = PickStatus();
                    var finalPremium = Math.Round((decimal)(_rng.NextDouble() * 2800 + 200), 2);

                    aggregates.Add(new PolicyReportAggregate
                    {
                        PolicyId = Guid.NewGuid(),
                        Country = loc.Country,
                        County = loc.County,
                        City = loc.City,
                        BrokerCode = brokerCode,
                        Currency = currency.Name,
                        Status = status,
                        BuildingType = buildingType,
                        FinalPremium = finalPremium,
                        FinalPremiumInBase = Math.Round(finalPremium * currency.ExchangeRateToBase, 2),
                        StartDate = startDate,
                        EndDate = endDate,
                        CreatedAt = createdAt
                    });
                }
            }

            for (int i = 0; i < 20; i++)
            {
                var daysBack = _rng.Next(0, 60);
                var startDate = now.AddDays(-daysBack);
                var loc = PrimaryLocations[_rng.Next(PrimaryLocations.Length)];
                var currency = currencies[_rng.Next(currencies.Count)];
                var finalPremium = Math.Round((decimal)(_rng.NextDouble() * 2800 + 200), 2);

                aggregates.Add(new PolicyReportAggregate
                {
                    PolicyId = Guid.NewGuid(),
                    Country = loc.Country,
                    County = loc.County,
                    City = loc.City,
                    BrokerCode = brokerCodes[_rng.Next(brokerCodes.Count)],
                    Currency = currency.Name,
                    Status = "UnderReview",
                    BuildingType = BuildingTypes[_rng.Next(BuildingTypes.Length)],
                    FinalPremium = finalPremium,
                    FinalPremiumInBase = Math.Round(finalPremium * currency.ExchangeRateToBase, 2),
                    StartDate = startDate,
                    EndDate = startDate.AddYears(1),
                    CreatedAt = startDate.AddDays(_rng.Next(-2, 1))
                });
            }

            _reportingDb.PolicyReportAggregates.AddRange(aggregates);
            await _reportingDb.SaveChangesAsync();
        }

        private static string PickStatus()
        {
            var total = StatusWeights.Sum(x => x.Weight);
            var roll = _rng.Next(total);
            var acc = 0;
            foreach (var (status, weight) in StatusWeights)
            {
                acc += weight;
                if (roll < acc) return status;
            }
            return "Active";
        }
    }
}
