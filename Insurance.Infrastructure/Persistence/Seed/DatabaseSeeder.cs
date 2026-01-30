using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder
    {
        private readonly InsuranceDbContext _context;

        public DatabaseSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await new GeographySeeder(_context).SeedAsync();
            await new CurrencySeeder(_context).SeedAsync();
            await new FeeConfigurationSeeder(_context).SeedAsync();
            await new RiskFactorConfigurationSeeder(_context).SeedAsync();
            await new BrokerSeeder(_context).SeedAsync();
            await new ClientSeeder(_context).SeedAsync();
            await new BuildingSeeder(_context).SeedAsync();
            await new BuildingRiskIndicatorSeeder(_context).SeedAsync();
        }
    }

}
