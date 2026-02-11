using Insurance.Domain.Buildings;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class BuildingGeoContextDto
    {
        public Guid BuildingId { get; set; }

        public Guid CityId { get; set; }
        public Guid CountyId { get; set; }
        public Guid CountryId { get; set; }

        public BuildingType BuildingType { get; set; }
        public IReadOnlyCollection<RiskIndicatorType> RiskIndicators { get; set; } = [];
    }

}
