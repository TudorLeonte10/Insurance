using Insurance.Domain.Clients;
using Insurance.Infrastructure.Persistence.Entities;
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

            var clients = new List<ClientEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
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
                Type = "Individual",
                Name = "Andrei Vasilescu",
                IdentificationNumber = "3456789012345",
                Email = "andrei@test.ro",
                PhoneNumber = "0755555555",
                Address = "Bucuresti"
            }
        };

            _context.Clients.AddRange(clients);
            await _context.SaveChangesAsync();
        }
    }


}



