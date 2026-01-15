using Insurance.Domain.Clients;
using Insurance.Domain.Geography;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.Buildings
{
    public class Building
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public City City { get; set; } = null!;
        public int ConstructionYear { get; set; }
        public BuildingType Type { get; set; }
        public double SurfaceArea { get; set; }
        public decimal InsuredValue { get; set; }
        public int NumberOfFloors { get; set; }
        public ICollection<BuildingRiskIndicator> RiskIndicators { get; set; } = new List<BuildingRiskIndicator>();
    }
}
