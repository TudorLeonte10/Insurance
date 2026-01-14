using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Geography
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountyId { get; set; }
        public County County { get; set; } = null!;

        public ICollection<Building> Buildings { get; set; } = new List<Building>();
    }
}
