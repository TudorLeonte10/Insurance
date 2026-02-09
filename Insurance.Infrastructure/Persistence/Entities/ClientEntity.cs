using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class ClientEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string IdentificationNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;

        public ICollection<BuildingEntity> Buildings { get; set; } = new List<BuildingEntity>();
    }
}
