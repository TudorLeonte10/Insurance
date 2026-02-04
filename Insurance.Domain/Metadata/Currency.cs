using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class Currency
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public decimal ExchangeRateToBase { get; set; }

        public bool IsActive { get; set; } = true;


    }
}
