using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    public class BuildingRiskIndicatorEntity
    {
        public Guid Id { get; set; }

        public Guid BuildingId { get; set; }
        public BuildingEntity Building { get; set; } = default!;

        public string RiskIndicator { get; set; } = default!;
    }
}
