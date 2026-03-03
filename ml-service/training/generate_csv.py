import os
from data_generator import generate_dataset

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
DATA_PATH = os.path.join(BASE_DIR, "train_data.csv")

if __name__ == "__main__":
    df = generate_dataset(
        n=6000,
        anomaly_rate=0.05,
        random_seed=42
    )

    df.to_csv(DATA_PATH, index=False)
    print("Saved train_data.csv with rows:", len(df))