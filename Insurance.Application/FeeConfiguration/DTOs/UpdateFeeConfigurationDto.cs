using Insurance.Domain.Metadata.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.DTOs
{
    public class UpdateFeeConfigurationDto
    {
        public string Name { get; set; } = string.Empty;
        public FeeType Type { get; set; }
        public decimal Percentage { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }

}
