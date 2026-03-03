import pandas as pd

df = pd.read_csv("train_data.csv")

normal = df[df["IsAnomalyLabel"] == 0]
underprice = df[df["AnomalyType"] == "UNDERPRICE"]

print("Normal ratio stats:")
print(normal["PremiumToInsuredValueRatio"].describe())

print("\nUnderprice ratio stats:")
print(underprice["PremiumToInsuredValueRatio"].describe())

