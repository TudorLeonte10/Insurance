using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(InsuranceDbContext context, IHostEnvironment environment)
        {
            if(!environment.IsDevelopment())
                return;

            if (environment.IsEnvironment("Test"))
                return;

            await GeographySeeder.SeedAsync(context);
            await ClientSeeder.SeedAsync(context);
            await BuildingSeeder.SeedAsync(context);
            await BuildingRiskIndicatorSeeder.SeedAsync(context);
        }
    }
}
