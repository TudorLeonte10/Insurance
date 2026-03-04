using Insurance.Application.Geography.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BuildingSummaryDto
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal InsuredValue { get; set; }

        public GeographyDto Geography { get; set; } = default!;
    }


}
