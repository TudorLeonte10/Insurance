import os
import numpy as np
import pandas as pd
import joblib

from sklearn.ensemble import IsolationForest
from sklearn.preprocessing import RobustScaler
from sklearn.metrics import classification_report, roc_auc_score


MODEL_DIR = "../models"
os.makedirs(MODEL_DIR, exist_ok=True)


def main():
    df = pd.read_csv("train_data.csv")

    feature_cols = [
        "FinalPremiumInBase",
        "InsuredValue",
        "PremiumToInsuredValueRatio",
        "LogPremiumToInsuredValueRatio",
        "BuildingAge",
        "ClientPoliciesLastYear",
        "PolicyDurationDays",
        "InsuredValuePerSquareMeter",
        "BrokerDeviationFromAverage",
        "ClientInsuredValueDerivationRatio",
        "ClientPremiumRatioDerivation",
    ]

    X = df[feature_cols]
    y = df["IsAnomalyLabel"]


    scaler = RobustScaler()
    X_scaled = scaler.fit_transform(X)


    model = IsolationForest(
        n_estimators=300,
        contamination=0.05,
        random_state=42
    )

    model.fit(X_scaled)

    scores = -model.decision_function(X_scaled)

    raw_predictions = model.predict(X_scaled)
    predictions = (raw_predictions == -1).astype(int)

    print("\nClassification Report:")
    print(classification_report(y, predictions))

    print("Predicted anomalies:", predictions.sum())
    print("Real anomalies:", y.sum())
    print("ROC-AUC:", roc_auc_score(y, scores))

    print("\nScore Distribution:")
    print("Min score:", scores.min())
    print("Max score:", scores.max())
    print("Mean score:", scores.mean())

    sorted_scores = np.sort(scores)

    joblib.dump(model, os.path.join(MODEL_DIR, "isolation_forest_model.joblib"))
    joblib.dump(scaler, os.path.join(MODEL_DIR, "scaler.joblib"))
    joblib.dump(feature_cols, os.path.join(MODEL_DIR, "feature_order.joblib"))
    joblib.dump(sorted_scores, os.path.join(MODEL_DIR, "training_scores.joblib"))

    print("\nModel and artifacts saved successfully.")
    print(np.percentile(scores, [90, 95, 97, 99]))


if __name__ == "__main__":
    main()