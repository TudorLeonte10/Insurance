using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Insurance.Application.Policy.DTOs
{
    public class PolicyMlResult
    {
        [JsonPropertyName("raw_score")]
        public decimal RawScore { get; set; }
        [JsonPropertyName("risk_score")]
        public decimal RiskScore { get; set; }
        [JsonPropertyName("is_anomaly")]
        public int IsAnomaly { get; set; }
    }
}
