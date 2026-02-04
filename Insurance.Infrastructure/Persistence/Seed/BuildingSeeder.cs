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
            var cities = _context.Cities.Take(5).ToList();

            var buildings = new List<BuildingEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = cities[0].Id,
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
                CityId = cities[1].Id,
                Street = "Str. Eroilor",
                Number = "25A",
                Type = "Office",
                ConstructionYear = 2015,
                NumberOfFloors = 4,
                SurfaceArea = 300,
                InsuredValue = 450000
            },
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = cities[2].Id,
                Street = "Bd. Aviatorilor",
                Number = "5",
                Type = "Residential",
                ConstructionYear = 1998,
                NumberOfFloors = 8,
                SurfaceArea = 500,
                InsuredValue = 600000
            },
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = cities[3].Id,
                Street = "Str. Victoriei",
                Number = "1",
                Type = "Office",
                ConstructionYear = 2010,
                NumberOfFloors = 6,
                SurfaceArea = 420,
                InsuredValue = 520000
            },
            new()
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = cities[4].Id,
                Street = "Str. Independentei",
                Number = "99",
                Type = "Residential",
                ConstructionYear = 1985,
                NumberOfFloors = 3,
                SurfaceArea = 180,
                InsuredValue = 200000
            }
        };

            _context.Buildings.AddRange(buildings);
            await _context.SaveChangesAsync();
        }
    }
    

}

