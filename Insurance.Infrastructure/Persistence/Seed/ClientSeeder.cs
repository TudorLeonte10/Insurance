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

            var client1 = Client.Create(
                ClientType.Individual,
                "Ion Popescu",
                "1234567890123",
                "ion.popescu@test.ro",
                "0712345678",
                "Str. Libertatii 10"
            );

            var client2 = Client.Create(
                ClientType.Company,
                "SC Test Construct SRL",
                "RO12345678",
                "office@testconstruct.ro",
                "0211234567",
                "Bd. Unirii 20"
            );

                

            var client3 = Client.Create(
                ClientType.Individual,
                "Maria Ionescu",
                "2980101123456",
                "maria.ionescu@test.ro",
                "0722333444",
                "Str. Florilor 5"
            );

            context.Clients.AddRange(client1, client2, client3);

            await context.SaveChangesAsync();
        }
    }
}



