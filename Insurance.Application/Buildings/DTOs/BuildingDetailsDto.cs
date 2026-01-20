using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.Geography;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class BuildingDetailsDto
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public BuildingType Type { get; set; }
        public int ConstructionYear { get; set; }
        public double SurfaceArea { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal InsuredValue { get; set; }
        public IReadOnlyList<string> RiskIndicators { get; set; } = new List<string>();
    }
}
