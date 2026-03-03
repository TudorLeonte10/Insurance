from fastapi import FastAPI
from .models import AnomalyFeatureDto, AnomalyScoreDto
from .score_service import score_policy

app = FastAPI(title="Insurance Anomaly Detection API")

@app.get("/health")
def health_check():
    return {"status": "healthy"}

@app.post("/score", response_model=AnomalyScoreDto)
def score(input: AnomalyFeatureDto):
    result = score_policy(input)
    return result