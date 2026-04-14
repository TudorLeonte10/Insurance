using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicySummaryDto
    {
        public int TotalPolicies { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal AveragePremium { get; set; }
        public int ActivePolicies { get; set; }
        public int UnderReviewPolicies { get; set; }
    }
}
