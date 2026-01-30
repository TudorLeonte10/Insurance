using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class Currency
    {
        public Guid Id { get; private set; }

        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;

        public decimal ExchangeRateToBase { get; private set; }

        public bool IsActive { get; private set; }
    }
}
