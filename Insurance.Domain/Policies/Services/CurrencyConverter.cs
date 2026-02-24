using Insurance.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public static class CurrencyConverter
    {
        public static decimal ConvertFromBase(decimal amountInBase, Currency currency)
        {
            return amountInBase / currency.ExchangeRateToBase;
        }
    }

}
