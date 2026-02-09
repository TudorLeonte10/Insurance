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

            var building = _context.Buildings.First();

            var indicators = new List<BuildingRiskIndicatorEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                BuildingId = building.Id,
                RiskIndicator = RiskIndicatorType.TheftRisk.ToString()
            },
            new()
            {
                Id = Guid.NewGuid(),
                BuildingId = building.Id,
                RiskIndicator = RiskIndicatorType.EarthquakeRisk.ToString()
            }
        };

            _context.BuildingRiskIndicators.AddRange(indicators);
            await _context.SaveChangesAsync();
        }
    }
}
