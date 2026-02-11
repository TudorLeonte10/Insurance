using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.DTOs
{
    public class UpdateCurrencyDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal ExchangeRateToBase { get; set; }
    }
}
