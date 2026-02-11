using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Persistence.Entities
{
    [ExcludeFromCodeCoverage]
    public class BuildingRiskIndicatorEntity
    {
        public Guid Id { get; set; }

        public Guid BuildingId { get; set; }
        public BuildingEntity Building { get; set; } = default!;

        public string RiskIndicator { get; set; } = default!;
    }
}
