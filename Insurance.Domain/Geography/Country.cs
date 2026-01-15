using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Geography
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<County> Counties { get; set; } = new List<County>();
    }
}
