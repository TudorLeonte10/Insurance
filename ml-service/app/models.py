from pydantic import BaseModel

class AnomalyFeatureDto(BaseModel):
    final_premium_in_base: float
    insured_value: float
    premium_to_insured_value_ratio: float
    building_age: int
    client_policies_last_year: int
    policy_duration_days: int
    insured_value_per_square_meter: float
    broker_deviation_from_average: float
    client_insured_value_derivation_ratio: float
    client_premium_ratio_derivation: float
    
class AnomalyScoreDto(BaseModel):
    raw_score: float
    risk_score: float
    is_anomaly: int