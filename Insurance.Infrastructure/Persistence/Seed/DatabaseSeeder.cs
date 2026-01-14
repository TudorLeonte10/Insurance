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

            await GeographySeeder.SeedAsync(context);
        }
    }
}
