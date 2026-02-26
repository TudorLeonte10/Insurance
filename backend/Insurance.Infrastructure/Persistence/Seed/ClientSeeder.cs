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

            var brokers = await _context.Brokers
                .OrderBy(b => b.BrokerCode)
                .ToListAsync();

            if (!brokers.Any())
                return;

            var defaultBroker = brokers.FirstOrDefault(b => b.BrokerCode == "BRK001")!;
            var premiumBroker = brokers.FirstOrDefault(b => b.BrokerCode == "BRK002")!;
            var inactiveBroker = brokers.FirstOrDefault(b => b.BrokerCode == "BRK003")!;
            var cityBroker = brokers.FirstOrDefault(b => b.BrokerCode == "BRK004")!;
            var regionalBroker = brokers.FirstOrDefault(b => b.BrokerCode == "BRK005")!;

            var clients = new List<ClientEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = defaultBroker.Id,
                    Type = "Individual",
                    Name = "Ion Popescu",
                    IdentificationNumber = "1234567890123",
                    Email = "ion@test.ro",
                    PhoneNumber = "0711111111",
                    Address = "Cluj"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = defaultBroker.Id,
                    Type = "Individual",
                    Name = "Maria Ionescu",
                    IdentificationNumber = "2345678901234",
                    Email = "maria@test.ro",
                    PhoneNumber = "0722222222",
                    Address = "Bucuresti"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = defaultBroker.Id,
                    Type = "Company",
                    Name = "SC Alpha SRL",
                    IdentificationNumber = "RO123456",
                    Email = "office@alpha.ro",
                    PhoneNumber = "0733333333",
                    Address = "Cluj"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = premiumBroker.Id,
                    Type = "Company",
                    Name = "SC Beta SRL",
                    IdentificationNumber = "RO654321",
                    Email = "contact@beta.ro",
                    PhoneNumber = "0744444444",
                    Address = "Iasi"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = premiumBroker.Id,
                    Type = "Individual",
                    Name = "Andrei Vasilescu",
                    IdentificationNumber = "3456789012345",
                    Email = "andrei@test.ro",
                    PhoneNumber = "0755555555",
                    Address = "Bucuresti"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = inactiveBroker.Id,
                    Type = "Individual",
                    Name = "Elena Dumitrescu",
                    IdentificationNumber = "4567890123456",
                    Email = "elena@test.ro",
                    PhoneNumber = "0766666666",
                    Address = "Timisoara"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = cityBroker.Id,
                    Type = "Company",
                    Name = "SC Gamma SA",
                    IdentificationNumber = "RO789012",
                    Email = "office@gamma.ro",
                    PhoneNumber = "0777777777",
                    Address = "Brasov"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = cityBroker.Id,
                    Type = "Individual",
                    Name = "Cristian Popa",
                    IdentificationNumber = "5678901234567",
                    Email = "cristian@test.ro",
                    PhoneNumber = "0788888888",
                    Address = "Sibiu"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = regionalBroker.Id,
                    Type = "Company",
                    Name = "SC Delta Industries SRL",
                    IdentificationNumber = "RO345678",
                    Email = "contact@delta.ro",
                    PhoneNumber = "0799999999",
                    Address = "Constanta"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrokerId = regionalBroker.Id,
                    Type = "Individual",
                    Name = "Alexandra Mihai",
                    IdentificationNumber = "6789012345678",
                    Email = "alexandra@test.ro",
                    PhoneNumber = "0700000001",
                    Address = "Galati"
                }
            };

            _context.Clients.AddRange(clients);
            await _context.SaveChangesAsync();
        }
    }


}



