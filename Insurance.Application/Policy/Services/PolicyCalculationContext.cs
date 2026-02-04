using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public class PolicyCalculationContext
    {
        public Guid CityId { get; init; }
        public Guid CountyId { get; init; }
        public Guid CountryId { get; init; }

        public BuildingType BuildingType { get; init; }
        public IReadOnlyCollection<RiskIndicatorType>? RiskIndicators { get; init; }
    }

}
