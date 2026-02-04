
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class GeographySeeder
    {
        private readonly InsuranceDbContext _context;

        public GeographySeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Countries.Any())
                return;

            var romania = new CountryEntity
            {
                Id = Guid.NewGuid(),
                Name = "Romania"
            };

            var cluj = new CountyEntity
            {
                Id = Guid.NewGuid(),
                Name = "Cluj",
                CountryId = romania.Id
            };

            var bucuresti = new CountyEntity
            {
                Id = Guid.NewGuid(),
                Name = "Bucuresti",
                CountryId = romania.Id
            };

            var iasi = new CountyEntity
            {
                Id = Guid.NewGuid(),
                Name = "Iasi",
                CountryId = romania.Id
            };

            var cities = new List<CityEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Cluj-Napoca", CountyId = cluj.Id },
            new() { Id = Guid.NewGuid(), Name = "Turda", CountyId = cluj.Id },

            new() { Id = Guid.NewGuid(), Name = "Sector 1", CountyId = bucuresti.Id },
            new() { Id = Guid.NewGuid(), Name = "Sector 3", CountyId = bucuresti.Id },

            new() { Id = Guid.NewGuid(), Name = "Iasi", CountyId = iasi.Id }
        };

            _context.Countries.Add(romania);
            _context.Counties.AddRange(cluj, bucuresti, iasi);
            _context.Cities.AddRange(cities);

            await _context.SaveChangesAsync();
        }
    }

}
