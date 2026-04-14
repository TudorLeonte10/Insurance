import os
import joblib
import pandas as pd
import xgboost as xgb

from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, roc_auc_score, confusion_matrix
from sklearn.calibration import CalibratedClassifierCV

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

    X_train, X_test, y_train, y_test = train_test_split(
        X,
        y,
        test_size=0.2,
        random_state=42,
        stratify=y
    )

    base_model = xgb.XGBClassifier(
        n_estimators=200,
        max_depth=6,         
        learning_rate=0.1,
        subsample=0.8,
        colsample_bytree=0.8,
        random_state=42,
        eval_metric="logloss"
    )

    model = CalibratedClassifierCV(base_model, method="sigmoid", cv=3)

    model.fit(X_train, y_train)

    y_proba = model.predict_proba(X_test)[:, 1]

    print("\nProba stats:")
    print("Min:", y_proba.min())
    print("Max:", y_proba.max())
    print("Mean:", y_proba.mean())

    thresholds = [0.3, 0.5, 0.7, 0.8, 0.9]

    for t in thresholds:
        print(f"\nThreshold: {t}")
        y_pred_threshold = (y_proba >= t).astype(int)

        print(classification_report(y_test, y_pred_threshold))
        print("ROC-AUC:", roc_auc_score(y_test, y_proba))
        print("Confusion Matrix:")
        print(confusion_matrix(y_test, y_pred_threshold))

    joblib.dump(model, os.path.join(MODEL_DIR, "xgboost_model.joblib"))
    joblib.dump(feature_cols, os.path.join(MODEL_DIR, "xgboost_feature_order.joblib"))



if __name__ == "__main__":
    main()