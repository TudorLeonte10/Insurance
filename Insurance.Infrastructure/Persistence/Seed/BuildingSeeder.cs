using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    public static class BuildingSeeder
    {
        public static async Task SeedAsync(InsuranceDbContext context)
        {
            if (context.Buildings.Any())
                return;

            // luăm date deja existente
            var clients = context.Clients.ToList();
            var cities = context.Cities.ToList();

            if (!clients.Any() || !cities.Any())
                return;

            var bucuresti = cities.First(c => c.Name == "Bucuresti");
            var iasi = cities.First(c => c.Name == "Iasi");

            var building1 = new Building
            {
                Id = Guid.NewGuid(),
                ClientId = clients[0].Id,
                CityId = bucuresti.Id,
                Street = "Calea Victoriei",
                Number = "15",
                ConstructionYear = 1990,
                Type = BuildingType.Residential,
                SurfaceArea = 120,
                NumberOfFloors = 2,
                InsuredValue = 120_000m
            };

            var building2 = new Building
            {
                Id = Guid.NewGuid(),
                ClientId = clients[0].Id,
                CityId = iasi.Id,
                Street = "Str. Stefan cel Mare",
                Number = "8",
                ConstructionYear = 2005,
                Type = BuildingType.Residential,
                SurfaceArea = 90,
                NumberOfFloors = 1,
                InsuredValue = 90_000m
            };

            var building3 = new Building
            {
                Id = Guid.NewGuid(),
                ClientId = clients[1].Id,
                CityId = bucuresti.Id,
                Street = "Bd. Unirii",
                Number = "45",
                ConstructionYear = 2010,
                Type = BuildingType.Office,
                SurfaceArea = 300,
                NumberOfFloors = 5,
                InsuredValue = 500_000m
            };

            context.Buildings.AddRange(building1, building2, building3);

            await context.SaveChangesAsync();
        }
    }
}

