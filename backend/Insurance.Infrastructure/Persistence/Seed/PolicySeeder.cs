using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class PolicySeeder
    {
        private readonly InsuranceDbContext _context;

        public PolicySeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Policies.Any())
                return;

            var rng = SeedRandom.Rng;

            var brokers = _context.Brokers.ToList();
            var clients = _context.Clients.ToList();
            var buildings = _context.Buildings.ToList();
            var currency = _context.Currencies.Single(c => c.Code == "EUR");

            var policies = new List<PolicyEntity>();

            foreach (var building in buildings)
            {
                var broker = brokers[rng.Next(brokers.Count)];
                var client = clients.Single(c => c.Id == building.ClientId);

                var durationOptions = new[] { 30, 90, 180, 365 };
                var duration = durationOptions[rng.Next(durationOptions.Length)];

                var buildingAge = DateTime.UtcNow.Year - building.ConstructionYear;

                var baseRate = 0.0035m + (buildingAge / 100m) * 0.0025m;

                var brokerMultiplier = (decimal)(0.8 + rng.NextDouble() * 0.6);

                var rate = baseRate * brokerMultiplier;

                var durationFactor = duration / 365m;

                var basePremium = building.InsuredValue * rate * durationFactor;

                var noise = (decimal)(0.9 + rng.NextDouble() * 0.2);

                var finalPremium = basePremium * noise;

                var startDate = DateTime.UtcNow.AddDays(-rng.Next(0, 200));

                policies.Add(new PolicyEntity
                {
                    Id = Guid.NewGuid(),
                    PolicyNumber = $"POL-{Guid.NewGuid():N}".Substring(0, 15),
                    ClientId = client.Id,
                    BuildingId = building.Id,
                    BrokerId = broker.Id,
                    CurrencyId = currency.Id,
                    BasePremium = basePremium,
                    FinalPremium = finalPremium,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(duration),
                    Status = PolicyStatus.Active,
                    CreatedAt = startDate.AddDays(-5),
                    ActivatedAt = startDate
                });
            }

            _context.Policies.AddRange(policies);
            await _context.SaveChangesAsync();
        }
    }
}
