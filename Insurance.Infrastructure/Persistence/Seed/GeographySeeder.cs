using Insurance.Domain.Geography;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    public static class GeographySeeder
    {
        public static async Task SeedAsync(InsuranceDbContext context)
        {
            if (context.Countries.Any())
                return;

            var romania = new Country
            {
                Id = Guid.NewGuid(),
                Name = "Romania",
                Counties = new List<County>()
            };

            var iasi = new County
            {
                Id = Guid.NewGuid(),
                Name = "Iasi",
                Country = romania,
                Cities = new List<City>()
            };

            var bucurestiCounty = new County
            {
                Id = Guid.NewGuid(),
                Name = "Bucuresti",
                Country = romania,
                Cities = new List<City>()
            };

            iasi.Cities.Add(new City
            {
                Id = Guid.NewGuid(),
                Name = "Iasi"
            });

            iasi.Cities.Add(new City
            {
                Id = Guid.NewGuid(),
                Name = "Pascani"
            });

            bucurestiCounty.Cities.Add(new City
            {
                Id = Guid.NewGuid(), 
                Name = "Bucuresti" 
            });

            romania.Counties.Add(iasi);
            romania.Counties.Add(bucurestiCounty);

            context.Countries.Add(romania);

            await context.SaveChangesAsync();
        }
    }

}
