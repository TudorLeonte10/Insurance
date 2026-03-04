import numpy as np
import pandas as pd


def generate_dataset(
    n: int = 5000,
    anomaly_rate: float = 0.05,
    random_seed: int = 42
) -> pd.DataFrame:

    rng = np.random.default_rng(random_seed)

    # -------------------------
    # Market assumptions
    # -------------------------

    city_price_eur_m2 = {
        "Bucharest": 2100,
        "Cluj-Napoca": 2400,
        "Timisoara": 1800,
        "Iasi": 1600,
        "Other": 1900,
    }

    duration_choices = np.array([30, 90, 180, 365])
    duration_probs = np.array([0.05, 0.05, 0.20, 0.70])

    # -------------------------
    # Brokers (mai variabili)
    # -------------------------

    n_brokers = 30
    broker_ids = np.arange(1, n_brokers + 1)

    # mai multă dispersie
    broker_scale = rng.lognormal(mean=0.0, sigma=0.40, size=n_brokers)

    # -------------------------
    # Clients
    # -------------------------

    n_clients = 4000
    client_ids = np.arange(1, n_clients + 1)

    client_policies_last_year = rng.poisson(lam=1.5, size=n_clients)
    client_policies_last_year = np.clip(client_policies_last_year, 0, 15)

    rows = []

    # -------------------------
    # Generate NORMAL data
    # -------------------------

    for _ in range(n):

        city = rng.choice(
            ["Bucharest", "Cluj-Napoca", "Timisoara", "Iasi", "Other"],
            p=[0.20, 0.12, 0.08, 0.15, 0.45]
        )

        base_price_m2 = city_price_eur_m2[city]

        surface_area = float(rng.lognormal(mean=4.6, sigma=0.50))
        surface_area = float(np.clip(surface_area, 25, 1500))

        # distribuție realistă a vechimii
        r = rng.random()
        if r < 0.20:
            building_age = int(rng.integers(0, 10))
        elif r < 0.75:
            building_age = int(rng.integers(10, 50))
        else:
            building_age = int(rng.integers(50, 90))

        # mai mult zgomot pe preț
        price_m2 = float(
            rng.normal(loc=base_price_m2, scale=base_price_m2 * 0.20)
        )
        price_m2 = float(max(price_m2, base_price_m2 * 0.5))

        insured_value = float(surface_area * price_m2)

        duration_days = int(
            rng.choice(duration_choices, p=duration_probs)
        )

        # rata de bază mai flexibilă
        base_rate = 0.0035 + (building_age / 100.0) * 0.0025

        broker_id = int(rng.choice(broker_ids))
        rate = base_rate * float(broker_scale[broker_id - 1])

        duration_factor = duration_days / 365.0

        final_premium = insured_value * rate * duration_factor

        # mai mult zgomot underwriting
        final_premium *= float(rng.normal(1.0, 0.12))
        final_premium = float(max(final_premium, 50.0))

        premium_ratio = final_premium / insured_value
        log_ratio = np.log(max(premium_ratio, 1e-8))
        premium_per_day = final_premium / duration_days
        insured_value_per_m2 = insured_value / surface_area

        client_id = int(rng.choice(client_ids))

        rows.append({
            "BrokerIdSim": broker_id,
            "ClientIdSim": client_id,
            "FinalPremiumInBase": final_premium,
            "PremiumPerDay": premium_per_day,
            "InsuredValue": insured_value,
            "PremiumToInsuredValueRatio": premium_ratio,
            "LogPremiumToInsuredValueRatio": log_ratio,
            "BuildingAge": float(building_age),
            "ClientPoliciesLastYear": int(
                client_policies_last_year[client_id - 1]
            ),
            "PolicyDurationDays": float(duration_days),
            "InsuredValuePerSquareMeter": insured_value_per_m2
        })

    df = pd.DataFrame(rows)

    # -------------------------
    # Baselines
    # -------------------------

    broker_avg_baseline = df.groupby(
        "BrokerIdSim"
    )["FinalPremiumInBase"].mean()

    client_avg_insured = df.groupby(
        "ClientIdSim"
    )["InsuredValue"].mean()

    client_avg_ratio = df.groupby(
        "ClientIdSim"
    )["PremiumToInsuredValueRatio"].mean()

    # -------------------------
    # Deviations
    # -------------------------

    df["BrokerDeviationFromAverage"] = (
        df["FinalPremiumInBase"]
        - df["BrokerIdSim"].map(broker_avg_baseline)
    )

    df["ClientInsuredValueDerivationRatio"] = (
        df["InsuredValue"]
        / df["ClientIdSim"].map(client_avg_insured)
    )

    df["ClientPremiumRatioDerivation"] = (
        df["PremiumToInsuredValueRatio"]
        / df["ClientIdSim"].map(client_avg_ratio)
    )

    df = df.fillna(1.0)

    # -------------------------
    # Inject realistic anomalies
    # -------------------------

    n_anom = int(round(n * anomaly_rate))
    anom_idx = rng.choice(df.index, size=n_anom, replace=False)

    anom_types = rng.choice(
        ["UNDERPRICE", "OVERPRICE", "VALUE_SHIFT", "BROKER_SHIFT"],
        size=n_anom,
        p=[0.30, 0.30, 0.25, 0.15]
    )

    df["IsAnomalyLabel"] = 0
    df["AnomalyType"] = None

    df.loc[anom_idx, "IsAnomalyLabel"] = 1
    df.loc[anom_idx, "AnomalyType"] = anom_types

    for idx, t in zip(anom_idx, anom_types):

        if t == "UNDERPRICE":
            df.at[idx, "FinalPremiumInBase"] *= rng.uniform(0.6, 0.8)

        elif t == "OVERPRICE":
            df.at[idx, "FinalPremiumInBase"] *= rng.uniform(1.3, 1.8)

        elif t == "VALUE_SHIFT":
            df.at[idx, "InsuredValue"] *= rng.uniform(1.5, 2.2)

        elif t == "BROKER_SHIFT":
            df.at[idx, "FinalPremiumInBase"] += rng.uniform(500, 2000)

        insured = max(df.at[idx, "InsuredValue"], 1)
        duration = max(df.at[idx, "PolicyDurationDays"], 1)

        df.at[idx, "PremiumToInsuredValueRatio"] = (
            df.at[idx, "FinalPremiumInBase"] / insured
        )

        df.at[idx, "LogPremiumToInsuredValueRatio"] = \
            np.log(max(df.at[idx, "PremiumToInsuredValueRatio"], 1e-8))

        df.at[idx, "PremiumPerDay"] = (
            df.at[idx, "FinalPremiumInBase"] / duration
        )

    return df