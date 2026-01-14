using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Clients
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ClientType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public ICollection<Building> Buildings { get; set; } = new List<Building>();

    }
}
