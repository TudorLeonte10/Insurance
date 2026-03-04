using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class CreateBuildingDto
    {
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public int ConstructionYear { get; set; }
        public BuildingType Type { get; set; }
        public decimal SurfaceArea { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal InsuredValue { get; set; }

        public IReadOnlyList<RiskIndicatorType> RiskIndicators { get; set; } = new List<RiskIndicatorType>();
    }
}
