
using Insurance.Infrastructure.Enums;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Diagnostics.CodeAnalysis;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class UserSeeder
    {
        private readonly InsuranceDbContext _context;

        public UserSeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Users.Any())
                return;

            var brokers = await _context.Brokers.ToListAsync();

            var users = new List<UserEntity>
        {
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@test.ro",
                PasswordHash = HashPassword("Admin123!"),
                Roles = UserRoles.Admin,
                BrokerId = null
            }
        };

            foreach (var broker in brokers)
            {
                users.Add(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Username = broker.BrokerCode.ToLower(),
                    Email = broker.Email,
                    PasswordHash = HashPassword("Broker123!"),
                    Roles = UserRoles.Broker,
                    BrokerId = broker.Id
                });
            }

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}
