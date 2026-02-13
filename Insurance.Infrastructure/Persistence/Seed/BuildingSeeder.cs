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

            var clients = _context.Clients.ToList();
            var cities = _context.Cities.ToList();

            var buildings = new List<BuildingEntity>();

            void AddBuilding(
                ClientEntity client,
                CityEntity city,
                string street,
                string number,
                BuildingType type,
                int year,
                int floors,
                decimal surface,
                decimal insuredValue)
            {
                buildings.Add(new BuildingEntity
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    CityId = city.Id,
                    Street = street,
                    Number = number,
                    Type = type,
                    ConstructionYear = year,
                    NumberOfFloors = floors,
                    SurfaceArea = surface,
                    InsuredValue = insuredValue
                });
            }

            var ion = clients.Single(c => c.Name == "Ion Popescu");

            AddBuilding(ion, cities.Single(c => c.Name == "Cluj-Napoca"),
                "Str. Memorandumului", "10", BuildingType.Residential, 2005, 2, 120, 150000);

            AddBuilding(ion, cities.Single(c => c.Name == "Turda"),
                "Str. Eroilor", "25A", BuildingType.Residential, 1998, 1, 90, 100000);

            AddBuilding(ion, cities.Single(c => c.Name == "Sector 1"),
                "Bd. Aviatorilor", "5", BuildingType.Office, 2012, 6, 400, 550000);

            var maria = clients.Single(c => c.Name == "Maria Ionescu");

            AddBuilding(maria, cities.Single(c => c.Name == "Sector 3"),
                "Str. Decebal", "45", BuildingType.Residential, 1985, 8, 300, 320000);

            AddBuilding(maria, cities.Single(c => c.Name == "Sector 6"),
                "Str. Apusului", "12", BuildingType.Residential, 2000, 4, 180, 210000);

            var alpha = clients.Single(c => c.Name == "SC Alpha SRL");

            AddBuilding(alpha, cities.Single(c => c.Name == "Cluj-Napoca"),
                "Str. Dorobanților", "99", BuildingType.Office, 2018, 5, 600, 800000);

            AddBuilding(alpha, cities.Single(c => c.Name == "Cluj-Napoca"),
                "Str. Horea", "15", BuildingType.Office, 2016, 3, 420, 620000);

            AddBuilding(alpha, cities.Single(c => c.Name == "Sector 1"),
                "Bd. Primăverii", "20", BuildingType.Office, 2019, 7, 900, 1200000);

            var beta = clients.Single(c => c.Name == "SC Beta SRL");

            AddBuilding(beta, cities.Single(c => c.Name == "Iasi"),
                "Str. Independenței", "1", BuildingType.Residential, 1990, 6, 350, 300000);

            AddBuilding(beta, cities.Single(c => c.Name == "Pascani"),
                "Str. Gării", "7", BuildingType.Residential, 1980, 2, 140, 130000);

            var andrei = clients.Single(c => c.Name == "Andrei Vasilescu");

            AddBuilding(andrei, cities.Single(c => c.Name == "Sector 1"),
                "Str. Paris", "3", BuildingType.Residential, 2008, 2, 160, 260000);

            _context.Buildings.AddRange(buildings);
            await _context.SaveChangesAsync();
        }
    }



}

