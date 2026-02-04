using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Seed
{
    [ExcludeFromCodeCoverage]
    public class CurrencySeeder
    {
        private readonly InsuranceDbContext _context;

        public CurrencySeeder(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Currencies.Any())
                return;

            var currencies = new List<CurrencyEntity>
        {
            new() { Id = Guid.NewGuid(), Code = "EUR", Name = "Euro", ExchangeRateToBase = 1m, IsActive = true },
            new() { Id = Guid.NewGuid(), Code = "USD", Name = "US Dollar", ExchangeRateToBase = 0.92m, IsActive = true },
            new() { Id = Guid.NewGuid(), Code = "GBP", Name = "Pound", ExchangeRateToBase = 1.20m, IsActive = true },
            new() { Id = Guid.NewGuid(), Code = "RON", Name = "Romanian Leu", ExchangeRateToBase = 0.20m, IsActive = true },
            new() { Id = Guid.NewGuid(), Code = "CHF", Name = "Swiss Franc", ExchangeRateToBase = 1.05m, IsActive = true }
        };

            _context.Currencies.AddRange(currencies);
            await _context.SaveChangesAsync();
        }
    }



}
