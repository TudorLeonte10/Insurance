using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.DTOs
{
    public class BuildingAnomalyContextDto
    {
        public decimal InsuredValue { get; set; }
        public decimal SurfaceArea { get; set; }
        public int BuildingAge { get; set; }
    }
}
