using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class CurrencyEntity
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public decimal ExchangeRateToBase { get; set; }

        public bool IsActive { get; set; }

        public ICollection<PolicyEntity> Policies { get; set; } = new List<PolicyEntity>();
    }

}
