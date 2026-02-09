using Insurance.Domain.Buildings;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class BuildingSeeder
    {
        private readonly InsuranceDbContext _context;

        public BuildingSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Buildings.Any())
                return;

            var client = _context.Clients.First();
            var city = _context.Cities.First();

            var buildings = new List<BuildingEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = city.Id,
                Street = "Str. Memorandumului",
                Number = "10",
                Type = "Residential",
                ConstructionYear = 2005,
                NumberOfFloors = 2,
                SurfaceArea = 120,
                InsuredValue = 150000
            },
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = city.Id,
                Street = "Str. Eroilor",
                Number = "25A",
                Type = "Office",
                ConstructionYear = 2015,
                NumberOfFloors = 4,
                SurfaceArea = 300,
                InsuredValue = 450000
            }
        };

            _context.Buildings.AddRange(buildings);
            await _context.SaveChangesAsync();
        }
    }

}

