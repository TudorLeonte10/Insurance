using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    [ExcludeFromCodeCoverage]
    public class BrokerEntity
    {
        public Guid Id { get; set; }

        public string BrokerCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PolicyEntity> Policies { get; set; } = new List<PolicyEntity>();
    }

}
