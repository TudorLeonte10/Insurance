using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class BuildingEntity
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public ClientEntity Client { get; set; } = default!;

        public Guid CityId { get; set; }
        public CityEntity City { get; set; } = default!;

        public string? Street { get; set; } 
        public string? Number { get; set; }
        public string? Type { get; set; } 
        public int ConstructionYear { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal SurfaceArea { get; set; }
        public decimal InsuredValue { get; set; }

        public ICollection<BuildingRiskIndicatorEntity> RiskIndicators { get; set; }
        = new List<BuildingRiskIndicatorEntity>();
    }
}
