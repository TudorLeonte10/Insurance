using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class ClientSeeder
    {
        private readonly InsuranceDbContext _context;

        public ClientSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Clients.Any())
                return;

            var brokers = _context.Brokers.ToList();
            var rng = SeedRandom.Rng;

            var clients = new List<ClientEntity>();

            for (int i = 0; i < 2000; i++)
            {
                var broker = brokers[rng.Next(brokers.Count)];

                clients.Add(new ClientEntity
                {
                    Id = Guid.NewGuid(),
                    BrokerId = broker.Id,
                    Type = rng.NextDouble() < 0.7 ? "Individual" : "Company",
                    Name = $"Client {i}",
                    IdentificationNumber = $"ID{i:0000000000}",
                    Email = $"client{i}@mail.ro",
                    PhoneNumber = $"07{rng.Next(10000000, 99999999)}",
                    Address = "Romania"
                });
            }

            _context.Clients.AddRange(clients);
            await _context.SaveChangesAsync();
        }
    }

}



