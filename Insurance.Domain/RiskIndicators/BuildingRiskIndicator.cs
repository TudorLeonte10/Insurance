using Insurance.Domain.Buildings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Domain.RiskIndicators
{
    public class BuildingRiskIndicator
    {
        public Guid BuildingId { get; set; }
        public Building Building { get; set; } = null!;

        public RiskIndicatorType RiskIndicator { get; set; }
    }
}
