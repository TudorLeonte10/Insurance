from score_service import score_policy

test_policy = {
    "FinalPremiumInBase": 8500,
    "InsuredValue": 250000,
    "PremiumToInsuredValueRatio": 0.034,
    "LogPremiumToInsuredValueRatio": -3.38,
    "BuildingAge": 35,
    "ClientPoliciesLastYear": 2,
    "PolicyDurationDays": 365,
    "InsuredValuePerSquareMeter": 1800,
    "BrokerDeviationFromAverage": 0.12,
    "ClientInsuredValueDerivationRatio": 1.1,
    "ClientPremiumRatioDerivation": 1.05
}

extreme_policy = {
    "FinalPremiumInBase": 45000,        # foarte mare
    "InsuredValue": 180000,             # relativ mic pentru premium-ul dat
    "PremiumToInsuredValueRatio": 0.25, # enorm (normal e ~0.003–0.01)
    "LogPremiumToInsuredValueRatio": -1.38,  
    "BuildingAge": 5,                   # nouă
    "ClientPoliciesLastYear": 10,       # mult peste medie
    "PolicyDurationDays": 30,           # durată scurtă
    "InsuredValuePerSquareMeter": 7000, # mult peste piață
    "BrokerDeviationFromAverage": 15000,# deviație mare
    "ClientInsuredValueDerivationRatio": 3.5,
    "ClientPremiumRatioDerivation": 4.0
}


result = score_policy(extreme_policy)

print("Extreme Policy Score:")
print(result)