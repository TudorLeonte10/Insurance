using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    [ExcludeFromCodeCoverage]
    public class CurrencyEntity
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public decimal ExchangeRateToBase { get; set; }

        public bool IsActive { get; set; }

        public ICollection<PolicyEntity> Policies { get; set; } = new List<PolicyEntity>();
    }

}
