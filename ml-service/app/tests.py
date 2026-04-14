from score_service import score_policy

test_policy = {
    "final_premium_in_base": 8500,
    "insured_value": 250000,
    "premium_to_insured_value_ratio": 0.034,
    "log_premium_to_insured_value_ratio": -3.38,
    "building_age": 31,
    "client_policies_last_year": 2,
    "policy_duration_days": 365,
    "insured_value_per_square_meter": 1800,
    "broker_deviation_from_average": 0.12,
    "client_insured_value_derivation_ratio": 1.1,
    "client_premium_ratio_derivation": 1.05
}

extreme_policy = {
    "final_premium_in_base": 45000,        # foarte mare
    "insured_value": 180000,             # relativ mic pentru premium-ul dat
    "premium_to_insured_value_ratio": 0.25, # enorm (normal e ~0.003–0.01)
    "log_premium_to_insured_value_ratio": -1.38,  
    "building_age": 5,                   # nouă
    "client_policies_last_year": 10,       # mult peste medie
    "policy_duration_days": 30,           # durată scurtă
    "insured_value_per_square_meter": 7000, # mult peste piață
    "broker_deviation_from_average": 15000,# deviație mare
    "client_insured_value_derivation_ratio": 3.5,
    "client_premium_ratio_derivation": 4.0
}


result = score_policy(test_policy)

print("Test Policy Score:")
print(result)