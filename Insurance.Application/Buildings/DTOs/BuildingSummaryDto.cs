using Insurance.Application.Geography.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class BuildingSummaryDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal InsuredValue { get; set; }

        public GeographyDto Geography { get; set; } = default!;
    }


}
