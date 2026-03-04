using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Insurance.Application.Policy.DTOs
{
    public class AnomalyFeatureDto
    {
        [JsonPropertyName("final_premium_in_base")]
        public decimal FinalPremiumInBase { get; set; }
        [JsonPropertyName("insured_value")]
        public decimal InsuredValue { get; set; }
        [JsonPropertyName("premium_to_insured_value_ratio")]
        public decimal PremiumToInsuredValueRatio { get; set; }
        [JsonPropertyName("building_age")]
        public int BuildingAge { get; set; }
        [JsonPropertyName("client_policies_last_year")]
        public int ClientPoliciesLastYear { get; set; }
        [JsonPropertyName("policy_duration_days")]
        public int PolicyDurationDays { get; set; }
        [JsonPropertyName("insured_value_per_square_meter")]
        public decimal InsuredValuePerSquareMeter { get; set; }
        [JsonPropertyName("broker_deviation_from_average")]
        public decimal BrokerDeviationFromAverage { get; set; }
        [JsonPropertyName("client_insured_value_derivation_ratio")]
        public decimal ClientInsuredValueDerivationRatio { get; set; }
        [JsonPropertyName("client_premium_ratio_derivation")]
        public decimal ClientPremiumRatioDerivation { get; set; }

    }
}
