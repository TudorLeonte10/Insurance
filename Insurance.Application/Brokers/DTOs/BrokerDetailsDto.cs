using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Application.Brokers.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BrokerDetailsDto
    {
        public Guid Id { get; set; }
        public string BrokerCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
