using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class BrokerSeeder
    {
        private readonly InsuranceDbContext _context;

        public BrokerSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Brokers.Any())
                return;

            var brokers = new List<BrokerEntity>();

            for (int i = 1; i <= 30; i++)
            {
                brokers.Add(new BrokerEntity
                {
                    Id = Guid.NewGuid(),
                    BrokerCode = $"BRK{i:000}",
                    Name = $"Broker {i}",
                    Email = $"broker{i}@test.ro",
                    Phone = $"07{i:0000000}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _context.Brokers.AddRange(brokers);
            await _context.SaveChangesAsync();
        }
    }
}
