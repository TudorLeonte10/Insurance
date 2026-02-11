using Insurance.Domain.Policies;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class PolicySeeder
    {
        private readonly InsuranceDbContext _context;

        public PolicySeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Policies.Any())
                return;

            var brokers = _context.Brokers.Where(b => b.IsActive).ToList();
            var inactiveBroker = _context.Brokers.Single(b => !b.IsActive);

            var clients = _context.Clients.ToList();
            var buildings = _context.Buildings.ToList();

            var currency = _context.Currencies.Single(c => c.Code == "EUR");

            var now = DateTime.UtcNow;

            var policies = new List<PolicyEntity>();

            void AddPolicy(
                BuildingEntity building,
                ClientEntity client,
                BrokerEntity broker,
                PolicyStatus status,
                decimal basePremium,
                decimal finalPremium,
                DateTime start,
                DateTime end,
                string? cancellationReason = null)
            {
                policies.Add(new PolicyEntity
                {
                    Id = Guid.NewGuid(),
                    PolicyNumber = $"POL-{Guid.NewGuid():N}".Substring(0, 15),
                    ClientId = client.Id,
                    BuildingId = building.Id,
                    BrokerId = broker.Id,
                    CurrencyId = currency.Id,
                    BasePremium = basePremium,
                    FinalPremium = finalPremium,
                    StartDate = start,
                    EndDate = end,
                    Status = status,
                    CreatedAt = start.AddDays(-10),
                    ActivatedAt = status == PolicyStatus.Active || status == PolicyStatus.Expired
                        ? start
                        : null,
                    CancelledAt = status == PolicyStatus.Cancelled
                        ? start.AddMonths(2)
                        : null,
                    CancellationReason = cancellationReason
                });
            }

            AddPolicy(buildings[0], clients[0], brokers[0],
                PolicyStatus.Active,
                1000, 1320,
                now.AddDays(-30), now.AddMonths(11));

            AddPolicy(buildings[1], clients[0], brokers[1],
                PolicyStatus.Active,
                800, 1050,
                now.AddDays(-60), now.AddMonths(10));

            AddPolicy(buildings[2], clients[1], brokers[0],
                PolicyStatus.Active,
                1200, 1650,
                now.AddDays(-15), now.AddMonths(12));

            AddPolicy(buildings[3], clients[2], brokers[2],
                PolicyStatus.Active,
                2000, 2750,
                now.AddDays(-45), now.AddMonths(8));

            AddPolicy(buildings[4], clients[3], brokers[1],
                PolicyStatus.Cancelled,
                900, 1100,
                now.AddMonths(-6), now.AddMonths(6),
                "Client requested cancellation");

            AddPolicy(buildings[5], clients[3], brokers[0],
                PolicyStatus.Cancelled,
                1500, 1900,
                now.AddMonths(-4), now.AddMonths(8),
                "Building sold");

            AddPolicy(buildings[6], clients[2], brokers[0],
                PolicyStatus.Expired,
                700, 900,
                now.AddYears(-1), now.AddDays(-10));

            AddPolicy(buildings[7], clients[1], brokers[1],
                PolicyStatus.Expired,
                1100, 1400,
                now.AddYears(-1), now.AddMonths(-1));

            AddPolicy(buildings[8], clients[4], brokers[2],
                PolicyStatus.Draft,
                600, 0,
                now.AddDays(10), now.AddMonths(12));

            AddPolicy(buildings[9], clients[4], brokers[0],
                PolicyStatus.Draft,
                1300, 0,
                now.AddDays(20), now.AddMonths(12));

            foreach (var building in buildings.Take(5))
            {
                AddPolicy(
                    building,
                    clients[0],
                    brokers[0],
                    PolicyStatus.Active,
                    500,
                    650,
                    now.AddDays(-90),
                    now.AddMonths(6));
            }

            _context.Policies.AddRange(policies);
            await _context.SaveChangesAsync();
        }
    }
}
