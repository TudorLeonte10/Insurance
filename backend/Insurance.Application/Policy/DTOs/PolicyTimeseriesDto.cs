using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyTimeseriesDto
    {
        public DateTime Date { get; set; }
        public int PolicyCount { get; set; }
        public decimal TotalPremium { get; set; }
    }
}
