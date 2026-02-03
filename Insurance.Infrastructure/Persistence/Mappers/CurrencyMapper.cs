using Insurance.Domain.Metadata;
using Insurance.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Mappers
{
    public static class CurrencyMapper
    {
        public static CurrencyEntity ToEntity(Currency currency)
        {
            return new CurrencyEntity
            {
                Id = currency.Id,
                Code = currency.Code,
                Name = currency.Name,
                ExchangeRateToBase = currency.ExchangeRateToBase,
                IsActive = currency.IsActive
            };
        }
        public static Currency ToDomain(CurrencyEntity entity)
        {
            return new Currency
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                ExchangeRateToBase = entity.ExchangeRateToBase,
                IsActive = entity.IsActive
            };
        }
    }
}
