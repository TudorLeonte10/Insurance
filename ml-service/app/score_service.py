import joblib
import os
import pandas as pd
import numpy as np

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
MODEL_DIR = os.path.join(BASE_DIR, "models") 

model = joblib.load(os.path.join(MODEL_DIR, "isolation_forest_model.joblib"))
scaler = joblib.load(os.path.join(MODEL_DIR, "scaler.joblib"))
feature_order = joblib.load(os.path.join(MODEL_DIR, "feature_order.joblib"))
training_scores = joblib.load(os.path.join(MODEL_DIR, "training_scores.joblib"))


def score_policy(dto):
    """
    dto can be:
        - dict
        - Pydantic model (with .dict() / .model_dump())
        - object with attributes
    """

    if isinstance(dto, dict):
        data = dto
    elif hasattr(dto, "model_dump"):  
        data = dto.model_dump()
    elif hasattr(dto, "dict"):  
        data = dto.dict()
    else:
        data = dto.__dict__
        
    data["FinalPremiumInBase"] = data["final_premium_in_base"]
    data["InsuredValue"] = data["insured_value"]
    data["PremiumToInsuredValueRatio"] = data["premium_to_insured_value_ratio"]
    data["BuildingAge"] = data["building_age"]
    data["ClientPoliciesLastYear"] = data["client_policies_last_year"]
    data["PolicyDurationDays"] = data["policy_duration_days"]
    data["InsuredValuePerSquareMeter"] = data["insured_value_per_square_meter"]
    data["BrokerDeviationFromAverage"] = data["broker_deviation_from_average"]
    data["ClientInsuredValueDerivationRatio"] = data["client_insured_value_derivation_ratio"]
    data["ClientPremiumRatioDerivation"] = data["client_premium_ratio_derivation"]
            
    data["LogPremiumToInsuredValueRatio"] = np.log(
    max(data["PremiumToInsuredValueRatio"], 1e-6))

    missing = [col for col in feature_order if col not in data]
    if missing:
        raise ValueError(f"Missing required features: {missing}")

    
    features_df = pd.DataFrame(
        [[data[col] for col in feature_order]],
        columns=feature_order
    )


    features_scaled = scaler.transform(features_df)

    raw_score = -model.decision_function(features_scaled)[0]

    percentile = (
        np.searchsorted(training_scores, raw_score, side="right")
        / len(training_scores)
    ) * 100

    risk_score = percentile

    is_anomaly = int(percentile > 98)

    return {
        "raw_score": float(raw_score),
        "risk_score": float(risk_score),
        "is_anomaly": is_anomaly
    }