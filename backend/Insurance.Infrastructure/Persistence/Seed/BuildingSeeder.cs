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

            var rng = SeedRandom.Rng;

            var clients = _context.Clients.ToList();
            var cities = _context.Cities.ToList();

            var cityPricePerM2 = new Dictionary<string, decimal>
        {
            { "Cluj-Napoca", 2400m },
            { "Sector 1", 2100m },
            { "Sector 3", 2100m },
            { "Sector 6", 2000m },
            { "Iasi", 1600m },
            { "Pascani", 1400m },
            { "Turda", 1500m }
        };

            var buildings = new List<BuildingEntity>();

            foreach (var client in clients)
            {
                // fiecare client are 1–3 clădiri
                int buildingCount = rng.Next(1, 4);

                for (int i = 0; i < buildingCount; i++)
                {
                    var city = cities[rng.Next(cities.Count)];

                    var basePrice = cityPricePerM2.ContainsKey(city.Name)
                        ? cityPricePerM2[city.Name]
                        : 1800m;

                    // Surface lognormal approx
                    var surface = (decimal)(Math.Exp(rng.NextDouble() * 1.5 + 3.5));
                    surface = Math.Clamp(surface, 30m, 1500m);

                    // price variation ±20%
                    var pricePerM2 = basePrice *
                                     (decimal)(0.8 + rng.NextDouble() * 0.4);

                    var insuredValue = surface * pricePerM2;

                    int constructionYear = rng.Next(1970, DateTime.UtcNow.Year);

                    buildings.Add(new BuildingEntity
                    {
                        Id = Guid.NewGuid(),
                        ClientId = client.Id,
                        CityId = city.Id,
                        Street = $"Street {rng.Next(1, 200)}",
                        Number = rng.Next(1, 100).ToString(),
                        Type = rng.NextDouble() < 0.7
                            ? BuildingType.Residential
                            : BuildingType.Office,
                        ConstructionYear = constructionYear,
                        NumberOfFloors = rng.Next(1, 10),
                        SurfaceArea = surface,
                        InsuredValue = insuredValue
                    });
                }
            }

            _context.Buildings.AddRange(buildings);
            await _context.SaveChangesAsync();
        }
    }
}

