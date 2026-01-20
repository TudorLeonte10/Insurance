using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class UpdateBuildingDto
    {
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int ConstructionYear { get; set; }
        public double SurfaceArea { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal InsuredValue { get; set; }

        public IReadOnlyList<RiskIndicatorType> RiskIndicators { get; set; } = new List<RiskIndicatorType>();
    }
}
