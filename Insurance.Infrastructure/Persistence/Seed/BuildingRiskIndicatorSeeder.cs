using Insurance.Domain.RiskIndicators;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class BuildingRiskIndicatorSeeder
    {
        private readonly InsuranceDbContext _context;

        public BuildingRiskIndicatorSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.BuildingRiskIndicators.Any())
                return;

            var buildings = _context.Buildings.Take(3).ToList();

            var indicators = new List<BuildingRiskIndicatorEntity>
        {
            new() { Id = Guid.NewGuid(), BuildingId = buildings[0].Id, RiskIndicator = RiskIndicatorType.EarthquakeRisk.ToString() },
            new() { Id = Guid.NewGuid(), BuildingId = buildings[0].Id, RiskIndicator = RiskIndicatorType.TheftRisk.ToString() },

            new() { Id = Guid.NewGuid(), BuildingId = buildings[1].Id, RiskIndicator = RiskIndicatorType.FireRisk.ToString() },

            new() { Id = Guid.NewGuid(), BuildingId = buildings[2].Id, RiskIndicator = RiskIndicatorType.FloodRisk.ToString() },
            new() { Id = Guid.NewGuid(), BuildingId = buildings[2].Id, RiskIndicator = RiskIndicatorType.EarthquakeRisk.ToString() }
        };

            _context.BuildingRiskIndicators.AddRange(indicators);
            await _context.SaveChangesAsync();
        }
    }

}
