using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    public static class BuildingRiskIndicatorSeeder
    {
        public static async Task SeedAsync(InsuranceDbContext context)
        {
            if (context.RiskIndicators.Any())
                return;

            var buildings = context.Buildings.ToList();

            if (!buildings.Any())
                return;

            var building1 = buildings.First();
            var building2 = buildings.Skip(1).FirstOrDefault();

            context.RiskIndicators.AddRange(
                new BuildingRiskIndicator
                {
                    BuildingId = building1.Id,
                    RiskIndicator = RiskIndicatorType.FloodRisk
                },
                new BuildingRiskIndicator
                {
                    BuildingId = building1.Id,
                    RiskIndicator = RiskIndicatorType.EarthquakeRisk
                }
            );

            if (building2 != null)
            {
                context.RiskIndicators.Add(
                    new BuildingRiskIndicator
                    {
                        BuildingId = building2.Id,
                        RiskIndicator = RiskIndicatorType.FireRisk
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
