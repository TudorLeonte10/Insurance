using Insurance.Domain.Metadata.Enums;
using Insurance.Domain.RiskIndicators;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class FeeConfigurationSeeder
    {
        private readonly InsuranceDbContext _context;

        public FeeConfigurationSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.FeeConfigurations.Any())
                return;

            var now = DateTime.UtcNow;

            var fees = new List<FeeConfigurationEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Admin fee",
                Type = FeeType.AdminFee,
                Percentage = 0.05m,
                EffectiveFrom = now,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Broker commission",
                Type = FeeType.BrokerCommission,
                Percentage = 0.10m,
                EffectiveFrom = now,
                IsActive = true
            }
        };

            foreach (var risk in Enum.GetValues<RiskIndicatorType>())
            {
                fees.Add(new FeeConfigurationEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"{risk} risk adjustment",
                    Type = FeeType.RiskAdjustment,
                    RiskIndicatorType = risk,
                    Percentage = GetDefaultRiskPercentage(risk),
                    EffectiveFrom = now,
                    IsActive = true
                });
            }

            _context.FeeConfigurations.AddRange(fees);
            await _context.SaveChangesAsync();
        }

        private static decimal GetDefaultRiskPercentage(RiskIndicatorType risk)
            => risk switch
            {
                RiskIndicatorType.FireRisk => 0.06m,
                RiskIndicatorType.FloodRisk => 0.08m,
                RiskIndicatorType.EarthquakeRisk => 0.12m,
                RiskIndicatorType.TheftRisk => 0.04m,
                _ => 0.05m
            };
    }

}
