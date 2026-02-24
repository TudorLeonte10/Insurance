using Insurance.Domain.Buildings;
using Insurance.Domain.Clients;
using Insurance.Domain.RiskIndicators;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BuildingDetailsDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int ConstructionYear { get; set; }
        public decimal SurfaceArea { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal InsuredValue { get; set; }
        public IReadOnlyList<string> RiskIndicators { get; set; } = new List<string>();
    }
}
