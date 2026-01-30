using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Insurance.Domain.Metadata
{
    public class FeeConfiguration
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public FeeType Type { get; private set; }

        public decimal Percentage { get; private set; }

        public DateTime EffectiveFrom { get; private set; }
        public DateTime? EffectiveTo { get; private set; }

        public bool IsActive { get; private set; }
    }
}
