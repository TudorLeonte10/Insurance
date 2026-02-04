using Insurance.Domain.Buildings;
using Insurance.Domain.Metadata.Enums;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class RiskFactorConfigurationSeeder
    {
        private readonly InsuranceDbContext _context;

        public RiskFactorConfigurationSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.RiskFactorConfigurations.Any())
                return;

            var risks = new List<RiskFactorConfigurationEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.Country,
                ReferenceId = _context.Countries.First().Id.ToString(),
                AdjustmentPercentage = 0.20m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.County,
                ReferenceId = _context.Counties.First().Id.ToString(),
                AdjustmentPercentage = 0.10m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.City,
                ReferenceId = _context.Cities.First().Id.ToString(),
                AdjustmentPercentage = 0.05m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.BuildingType,
                ReferenceId = ((int)BuildingType.Residential).ToString(),
                AdjustmentPercentage = 0.08m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.BuildingType,
                ReferenceId = ((int)BuildingType.Office).ToString(),
                AdjustmentPercentage = 0.12m,
                IsActive = true
            }
        };

            _context.RiskFactorConfigurations.AddRange(risks);
            await _context.SaveChangesAsync();
        }
    }


}


