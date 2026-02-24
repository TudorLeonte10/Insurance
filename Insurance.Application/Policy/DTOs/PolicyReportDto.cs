using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyReportDto
    {
        public string GroupName { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int PoliciesCount { get; set; } 
        public decimal TotalPremium { get; set; }
        public decimal TotalPremiumInBase { get; set; }
    }
}


