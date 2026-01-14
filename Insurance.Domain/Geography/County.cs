using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Geography
{
    public class County
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
