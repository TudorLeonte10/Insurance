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

            var fees = new List<FeeConfigurationEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Standard Broker Fee",
                Type = FeeType.ConfigurationFee,
                Percentage = 5m,
                EffectiveFrom = DateTime.UtcNow.AddYears(-1),
                IsActive = true
            }
        };

            _context.FeeConfigurations.AddRange(fees);
            await _context.SaveChangesAsync();
        }
    }


}
