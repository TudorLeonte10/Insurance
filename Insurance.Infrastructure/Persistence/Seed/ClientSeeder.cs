using Insurance.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    public static class ClientSeeder
    {
        public static async Task SeedAsync(InsuranceDbContext context)
        {
            if (context.Clients.Any())
                return;

            var client1 = new Client
            {
                Id = Guid.NewGuid(),
                Name = "Ion Popescu",
                IdentificationNumber = "1234567890123",
                Type = ClientType.Individual,
                Email = "ion.popescu@test.ro",
                PhoneNumber = "0712345678",
                Address = "Str. Libertatii 10"
            };

            var client2 = new Client
            {
                Id = Guid.NewGuid(),
                Name = "SC Test Construct SRL",
                IdentificationNumber = "RO12345678",
                Type = ClientType.Company,
                Email = "office@testconstruct.ro",
                PhoneNumber = "0211234567",
                Address = "Bd. Unirii 20"
            };

            var client3 = new Client
            {
                Id = Guid.NewGuid(),
                Name = "Maria Ionescu",
                IdentificationNumber = "2980101123456",
                Type = ClientType.Individual,
                Email = "maria.ionescu@test.ro",
                PhoneNumber = "0722333444",
                Address = "Str. Florilor 5"
            };

            context.Clients.AddRange(client1, client2, client3);

            await context.SaveChangesAsync();
        }
    }
}
