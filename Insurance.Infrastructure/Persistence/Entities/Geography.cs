using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class CountryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<CountyEntity> Counties { get; set; } = new List<CountyEntity>();
    }

    public class CountyEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public Guid CountryId { get; set; }
        public CountryEntity Country { get; set; } = default!;

        public ICollection<CityEntity> Cities { get; set; } = new List<CityEntity>();
    }

    public class CityEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public Guid CountyId { get; set; }
        public CountyEntity County { get; set; } = default!;

        public ICollection<BuildingEntity> Buildings { get; set; } = new List<BuildingEntity>();
    }
}
