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

            var romania = _context.Countries.Single(c => c.Name == "Romania");

            var cluj = _context.Counties.Single(c => c.Name == "Cluj");
            var bucuresti = _context.Counties.Single(c => c.Name == "Bucuresti");
            var iasi = _context.Counties.Single(c => c.Name == "Iasi");

            var clujNapoca = _context.Cities.Single(c => c.Name == "Cluj-Napoca");
            var sector1 = _context.Cities.Single(c => c.Name == "Sector 1");
            var turda = _context.Cities.Single(c => c.Name == "Turda");

            var risks = new List<RiskFactorConfigurationEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.Country,
                ReferenceId = romania.Id.ToString(),
                AdjustmentPercentage = 0.15m,
                IsActive = true
            },

            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.County,
                ReferenceId = bucuresti.Id.ToString(),
                AdjustmentPercentage = 0.20m, 
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.County,
                ReferenceId = cluj.Id.ToString(),
                AdjustmentPercentage = 0.10m, 
                IsActive = true
            },

            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.City,
                ReferenceId = clujNapoca.Id.ToString(),
                AdjustmentPercentage = 0.07m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.City,
                ReferenceId = sector1.Id.ToString(),
                AdjustmentPercentage = 0.12m,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Level = RiskFactorLevel.City,
                ReferenceId = turda.Id.ToString(),
                AdjustmentPercentage = 0.05m,
                IsActive = false 
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


