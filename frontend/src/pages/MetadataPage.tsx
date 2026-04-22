import { useEffect, useState } from "react";
import {
  createCurrency,
  createFeeConfiguration,
  createRiskFactor,
  getAllRiskFactors,
  getCurrencies,
  getFeeConfiguration,
  updateFeeConfiguration,
  updateRiskFactor,
} from "../api/metadataApi";
import {
  getCities,
  getCounties,
  getCountries,
  type City,
  type Country,
  type County,
} from "../api/locationApi";
import { BuildingType, RiskIndicator } from "../types/buildings";
import { FeeType, RiskFactorLevel } from "../types/metadata";

interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
}

interface Currency {
  id: string;
  code: string;
  name: string;
  exchangeRateToBase: number;
  isActive: boolean;
}

interface FeeConfiguration {
  id: string;
  name: string;
  type: FeeType;
  percentage: number;
  effectiveFrom: string;
  effectiveTo?: string | null;
  isActive: boolean;
  riskIndicator?: RiskIndicator | null;
}

interface RiskFactor {
  id: string;
  level: RiskFactorLevel;
  referenceId: string;
  adjustmentPercentage: number;
  isActive: boolean;
}

interface ApiFeeConfiguration {
  id: string;
  name: string;
  type: FeeType | string | number;
  percentage: number;
  effectiveFrom: string;
  effectiveTo?: string | null;
  isActive: boolean;
  riskIndicator?: RiskIndicator | string | number | null;
}

interface ApiRiskFactor {
  id: string;
  level: RiskFactorLevel | string | number;
  referenceId: string;
  adjustmentPercentage: number;
  isActive: boolean;
}

interface FeeDraft {
  name: string;
  type: FeeType;
  percentage: string;
  effectiveFrom: string;
  effectiveTo: string;
  isActive: boolean;
}

interface RiskDraft {
  level: RiskFactorLevel;
  referenceId: string;
  adjustmentPercentage: string;
  isActive: boolean;
  selectedCountryId: string;
  selectedCountyId: string;
}

const today = new Date().toISOString().slice(0, 10);

const feeTypeByApiValue: Record<string, FeeType> = {
  brokerCommission: FeeType.BrokerCommission,
  riskAdjustment: FeeType.RiskAdjustment,
  adminFee: FeeType.AdminFee,
};

const riskFactorLevelByApiValue: Record<string, RiskFactorLevel> = {
  country: RiskFactorLevel.Country,
  county: RiskFactorLevel.County,
  city: RiskFactorLevel.City,
  buildingType: RiskFactorLevel.BuildingType,
};

const riskIndicatorByApiValue: Record<string, RiskIndicator> = {
  fireRisk: RiskIndicator.FireRisk,
  floodRisk: RiskIndicator.FloodRisk,
  earthquakeRisk: RiskIndicator.EarthquakeRisk,
  theftRisk: RiskIndicator.TheftRisk,
};

function MetadataPage() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [locationWarning, setLocationWarning] = useState<string | null>(null);

  const [currencies, setCurrencies] = useState<Currency[]>([]);
  const [fees, setFees] = useState<FeeConfiguration[]>([]);
  const [riskFactors, setRiskFactors] = useState<RiskFactor[]>([]);

  const [countries, setCountries] = useState<Country[]>([]);
  const [countiesByCountry, setCountiesByCountry] = useState<Record<string, County[]>>({});
  const [citiesByCounty, setCitiesByCounty] = useState<Record<string, City[]>>({});
  const [countyToCountry, setCountyToCountry] = useState<Record<string, string>>({});
  const [cityToCounty, setCityToCounty] = useState<Record<string, string>>({});

  const [editingFeeId, setEditingFeeId] = useState<string | null>(null);
  const [editingRiskId, setEditingRiskId] = useState<string | null>(null);
  const [feeDraft, setFeeDraft] = useState<FeeDraft | null>(null);
  const [riskDraft, setRiskDraft] = useState<RiskDraft | null>(null);

  const [currencyForm, setCurrencyForm] = useState({
    code: "",
    name: "",
    exchangeRateToBase: "",
  });

  const [feeForm, setFeeForm] = useState({
    name: "",
    type: FeeType.BrokerCommission,
    percentage: "",
    effectiveFrom: today,
    effectiveTo: "",
  });

  const [riskForm, setRiskForm] = useState<RiskDraft>({
    level: RiskFactorLevel.Country,
    referenceId: "",
    adjustmentPercentage: "",
    isActive: true,
    selectedCountryId: "",
    selectedCountyId: "",
  });

  const loadAll = async () => {
    setError(null);
    setLoading(true);

    try {
      const [currencyRes, feeRes, riskRes] = await Promise.all([
        getCurrencies() as Promise<PagedResult<Currency>>,
        getFeeConfiguration() as Promise<PagedResult<ApiFeeConfiguration>>,
        getAllRiskFactors() as Promise<PagedResult<ApiRiskFactor>>,
      ]);

      setCurrencies(currencyRes.items ?? []);
      setFees((feeRes.items ?? []).map(normalizeFeeConfiguration));
      setRiskFactors((riskRes.items ?? []).map(normalizeRiskFactor));
    } catch (err) {
      console.error(err);
      setError("Nu s-au putut încărca datele de metadata.");
    } finally {
      setLoading(false);
    }
  };

  const loadLocations = async () => {
    try {
      const countriesResponse = (await getCountries()) as Country[];
      setCountries(countriesResponse);

      const countyEntries = await Promise.all(
        countriesResponse.map(async country => {
          const countiesResponse = (await getCounties(country.id)) as County[];
          return [country.id, countiesResponse] as const;
        })
      );

      const nextCountiesByCountry: Record<string, County[]> = {};
      const nextCountyToCountry: Record<string, string> = {};

      countyEntries.forEach(([countryId, countyList]) => {
        nextCountiesByCountry[countryId] = countyList;
        countyList.forEach(county => {
          nextCountyToCountry[county.id] = countryId;
        });
      });

      setCountiesByCountry(nextCountiesByCountry);
      setCountyToCountry(nextCountyToCountry);

      const allCounties = countyEntries.flatMap(([, countyList]) => countyList);
      const cityEntries = await Promise.all(
        allCounties.map(async county => {
          const citiesResponse = (await getCities(county.id)) as City[];
          return [county.id, citiesResponse] as const;
        })
      );

      const nextCitiesByCounty: Record<string, City[]> = {};
      const nextCityToCounty: Record<string, string> = {};

      cityEntries.forEach(([countyId, cityList]) => {
        nextCitiesByCounty[countyId] = cityList;
        cityList.forEach(city => {
          nextCityToCounty[city.id] = countyId;
        });
      });

      setCitiesByCounty(nextCitiesByCounty);
      setCityToCounty(nextCityToCounty);
      setLocationWarning(null);
    } catch (err) {
      console.error(err);
      setLocationWarning(
        "Nu s-au putut încărca locațiile pentru dropdown-urile de risk factor. Cel mai probabil endpoint-urile sunt accesibile doar pentru broker."
      );
    }
  };

  useEffect(() => {
    loadAll();
    loadLocations();
  }, []);

  const handleCreateCurrency = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      await createCurrency({
        code: currencyForm.code.trim(),
        name: currencyForm.name.trim(),
        exchangeRateToBase: Number(currencyForm.exchangeRateToBase),
      });

      setCurrencyForm({
        code: "",
        name: "",
        exchangeRateToBase: "",
      });

      await loadAll();
    } catch (err) {
      console.error(err);
      setError("Crearea monedei a eșuat.");
    } finally {
      setSaving(false);
    }
  };

  const handleCreateFee = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      await createFeeConfiguration({
        name: feeForm.name.trim(),
        type: Number(feeForm.type) as FeeType,
        percentage: Number(feeForm.percentage),
        effectiveFrom: feeForm.effectiveFrom,
        effectiveTo: feeForm.effectiveTo || undefined,
      });

      setFeeForm({
        name: "",
        type: FeeType.BrokerCommission,
        percentage: "",
        effectiveFrom: today,
        effectiveTo: "",
      });

      await loadAll();
    } catch (err) {
      console.error(err);
      setError("Crearea fee-ului a eșuat.");
    } finally {
      setSaving(false);
    }
  };

  const handleCreateRisk = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      const referenceId = resolveReferenceId(riskForm);
      if (!referenceId) {
        setError("Selectează o valoare validă pentru risk factor.");
        return;
      }

      await createRiskFactor({
        level: Number(riskForm.level) as RiskFactorLevel,
        referenceId,
        adjustmentPercentage: Number(riskForm.adjustmentPercentage),
      });

      setRiskForm({
        level: RiskFactorLevel.Country,
        referenceId: "",
        adjustmentPercentage: "",
        isActive: true,
        selectedCountryId: "",
        selectedCountyId: "",
      });

      await loadAll();
    } catch (err) {
      console.error(err);
      setError("Crearea risk factor-ului a eșuat.");
    } finally {
      setSaving(false);
    }
  };

  const startFeeEdit = (fee: FeeConfiguration) => {
    setEditingFeeId(fee.id);
    setFeeDraft({
      name: fee.name,
      type: fee.type,
      percentage: String(fee.percentage),
      effectiveFrom: toDateInput(fee.effectiveFrom),
      effectiveTo: fee.effectiveTo ? toDateInput(fee.effectiveTo) : "",
      isActive: fee.isActive,
    });
  };

  const cancelFeeEdit = () => {
    setEditingFeeId(null);
    setFeeDraft(null);
  };

  const handleUpdateFee = async (feeId: string) => {
    if (!feeDraft) {
      return;
    }

    setSaving(true);
    setError(null);

    try {
      await updateFeeConfiguration(feeId, {
        name: feeDraft.name,
        type: feeDraft.type,
        percentage: Number(feeDraft.percentage),
        effectiveFrom: feeDraft.effectiveFrom,
        effectiveTo: feeDraft.effectiveTo || undefined,
        isActive: feeDraft.isActive,
      });

      cancelFeeEdit();
      await loadAll();
    } catch (err) {
      console.error(err);
      setError("Actualizarea fee-ului a eșuat.");
    } finally {
      setSaving(false);
    }
  };

  const startRiskEdit = (risk: RiskFactor) => {
    setEditingRiskId(risk.id);
    setRiskDraft(buildRiskDraft(risk));
  };

  const cancelRiskEdit = () => {
    setEditingRiskId(null);
    setRiskDraft(null);
  };

  const handleUpdateRisk = async (riskId: string) => {
    if (!riskDraft) {
      return;
    }

    setSaving(true);
    setError(null);

    try {
      const referenceId = resolveReferenceId(riskDraft);
      if (!referenceId) {
        setError("Selectează o valoare validă pentru risk factor.");
        return;
      }

      await updateRiskFactor(riskId, {
        level: riskDraft.level,
        referenceId,
        adjustmentPercentage: Number(riskDraft.adjustmentPercentage),
        isActive: riskDraft.isActive,
      });

      cancelRiskEdit();
      await loadAll();
    } catch (err) {
      console.error(err);
      setError("Actualizarea risk factor-ului a eșuat.");
    } finally {
      setSaving(false);
    }
  };

  function buildRiskDraft(risk: RiskFactor): RiskDraft {
    if (risk.level === RiskFactorLevel.Country) {
      return {
        level: risk.level,
        referenceId: risk.referenceId,
        adjustmentPercentage: String(risk.adjustmentPercentage),
        isActive: risk.isActive,
        selectedCountryId: risk.referenceId,
        selectedCountyId: "",
      };
    }

    if (risk.level === RiskFactorLevel.County) {
      return {
        level: risk.level,
        referenceId: risk.referenceId,
        adjustmentPercentage: String(risk.adjustmentPercentage),
        isActive: risk.isActive,
        selectedCountryId: countyToCountry[risk.referenceId] ?? "",
        selectedCountyId: risk.referenceId,
      };
    }

    if (risk.level === RiskFactorLevel.City) {
      const countyId = cityToCounty[risk.referenceId] ?? "";
      return {
        level: risk.level,
        referenceId: risk.referenceId,
        adjustmentPercentage: String(risk.adjustmentPercentage),
        isActive: risk.isActive,
        selectedCountryId: countyToCountry[countyId] ?? "",
        selectedCountyId: countyId,
      };
    }

    return {
      level: risk.level,
      referenceId: risk.referenceId,
      adjustmentPercentage: String(risk.adjustmentPercentage),
      isActive: risk.isActive,
      selectedCountryId: "",
      selectedCountyId: "",
    };
  }

  const handleRiskLevelChange = (
    nextLevel: RiskFactorLevel,
    mode: "create" | "edit"
  ) => {
    if (mode === "create") {
      setRiskForm(prev => ({
        ...prev,
        level: nextLevel,
        referenceId: "",
        selectedCountryId: "",
        selectedCountyId: "",
      }));
      return;
    }

    setRiskDraft(prev =>
      prev
        ? {
            ...prev,
            level: nextLevel,
            referenceId: "",
            selectedCountryId: "",
            selectedCountyId: "",
          }
        : prev
    );
  };

  const handleRiskCountryChange = (
    countryId: string,
    mode: "create" | "edit"
  ) => {
    if (mode === "create") {
      setRiskForm(prev => ({
        ...prev,
        selectedCountryId: countryId,
        selectedCountyId: "",
        referenceId:
          prev.level === RiskFactorLevel.Country ? countryId : "",
      }));
      return;
    }

    setRiskDraft(prev =>
      prev
        ? {
            ...prev,
            selectedCountryId: countryId,
            selectedCountyId: "",
            referenceId:
              prev.level === RiskFactorLevel.Country ? countryId : "",
          }
        : prev
    );
  };

  const handleRiskCountyChange = (
    countyId: string,
    mode: "create" | "edit"
  ) => {
    if (mode === "create") {
      setRiskForm(prev => ({
        ...prev,
        selectedCountyId: countyId,
        referenceId:
          prev.level === RiskFactorLevel.County ? countyId : "",
      }));
      return;
    }

    setRiskDraft(prev =>
      prev
        ? {
            ...prev,
            selectedCountyId: countyId,
            referenceId:
              prev.level === RiskFactorLevel.County ? countyId : "",
          }
        : prev
    );
  };

  const handleRiskReferenceChange = (
    referenceId: string,
    mode: "create" | "edit"
  ) => {
    if (mode === "create") {
      setRiskForm(prev => ({ ...prev, referenceId }));
      return;
    }

    setRiskDraft(prev => (prev ? { ...prev, referenceId } : prev));
  };

  const resolveReferenceId = (draft: RiskDraft) => {
    if (draft.level === RiskFactorLevel.Country) {
      return draft.selectedCountryId;
    }

    if (draft.level === RiskFactorLevel.County) {
      return draft.selectedCountyId;
    }

    return draft.referenceId;
  };

  const getCountiesForCountry = (countryId: string) => {
    return countiesByCountry[countryId] ?? [];
  };

  const getCitiesForCounty = (countyId: string) => {
    return citiesByCounty[countyId] ?? [];
  };

  const getFeeTypeLabel = (value: FeeType) => FeeType[value];
  const getRiskLevelLabel = (value: RiskFactorLevel) => RiskFactorLevel[value];
  const getRiskIndicatorLabel = (value?: RiskIndicator | null) => {
    if (!value) {
      return "-";
    }

    return RiskIndicator[value];
  };
  const getBuildingTypeLabel = (value: string) => {
    const numericValue = Number(value);
    return Number.isNaN(numericValue)
      ? value
      : BuildingType[numericValue as BuildingType] ?? value;
  };

  const getReferenceLabel = (risk: RiskFactor) => {
    if (risk.level === RiskFactorLevel.BuildingType) {
      return getBuildingTypeLabel(risk.referenceId);
    }

    if (risk.level === RiskFactorLevel.Country) {
      return countries.find(country => country.id === risk.referenceId)?.name ?? risk.referenceId;
    }

    if (risk.level === RiskFactorLevel.County) {
      const counties = Object.values(countiesByCountry).flat();
      return counties.find(county => county.id === risk.referenceId)?.name ?? risk.referenceId;
    }

    const cities = Object.values(citiesByCounty).flat();
    return cities.find(city => city.id === risk.referenceId)?.name ?? risk.referenceId;
  };

  const renderRiskReferenceSelectors = (
    draft: RiskDraft,
    mode: "create" | "edit"
  ) => {
    if (draft.level === RiskFactorLevel.BuildingType) {
      return (
        <select
          className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
          value={draft.referenceId}
          onChange={e => handleRiskReferenceChange(e.target.value, mode)}
        >
          <option value="">Select building type</option>
          {Object.entries(BuildingType)
            .filter(([, value]) => typeof value === "number")
            .map(([label, value]) => (
              <option key={value} value={String(value)}>
                {label}
              </option>
            ))}
        </select>
      );
    }

    if (draft.level === RiskFactorLevel.Country) {
      return (
        <select
          className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
          value={draft.selectedCountryId}
          onChange={e => handleRiskCountryChange(e.target.value, mode)}
        >
          <option value="">Select country</option>
          {countries.map(country => (
            <option key={country.id} value={country.id}>
              {country.name}
            </option>
          ))}
        </select>
      );
    }

    if (draft.level === RiskFactorLevel.County) {
      return (
        <>
          <select
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={draft.selectedCountryId}
            onChange={e => handleRiskCountryChange(e.target.value, mode)}
          >
            <option value="">Select country</option>
            {countries.map(country => (
              <option key={country.id} value={country.id}>
                {country.name}
              </option>
            ))}
          </select>

          <select
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={draft.selectedCountyId}
            onChange={e => handleRiskCountyChange(e.target.value, mode)}
            disabled={!draft.selectedCountryId}
          >
            <option value="">Select county</option>
            {getCountiesForCountry(draft.selectedCountryId).map(county => (
              <option key={county.id} value={county.id}>
                {county.name}
              </option>
            ))}
          </select>
        </>
      );
    }

    return (
      <>
        <select
          className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
          value={draft.selectedCountryId}
          onChange={e => handleRiskCountryChange(e.target.value, mode)}
        >
          <option value="">Select country</option>
          {countries.map(country => (
            <option key={country.id} value={country.id}>
              {country.name}
            </option>
          ))}
        </select>

        <select
          className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
          value={draft.selectedCountyId}
          onChange={e => handleRiskCountyChange(e.target.value, mode)}
          disabled={!draft.selectedCountryId}
        >
          <option value="">Select county</option>
          {getCountiesForCountry(draft.selectedCountryId).map(county => (
            <option key={county.id} value={county.id}>
              {county.name}
            </option>
          ))}
        </select>

        <select
          className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
          value={draft.referenceId}
          onChange={e => handleRiskReferenceChange(e.target.value, mode)}
          disabled={!draft.selectedCountyId}
        >
          <option value="">Select city</option>
          {getCitiesForCounty(draft.selectedCountyId).map(city => (
            <option key={city.id} value={city.id}>
              {city.name}
            </option>
          ))}
        </select>
      </>
    );
  };

  if (loading) {
    return (
      <div className="space-y-4">
        <div className="h-8 w-48 bg-slate-200 rounded animate-pulse" />
        <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
        <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">Metadata</h1>
        <p className="text-sm text-slate-500 mt-0.5">
          Manage currencies, fee configurations and risk factors.
        </p>
      </div>

      {error && (
        <div className="flex items-center gap-2 bg-red-50 border border-red-200 text-red-700 text-sm px-4 py-3 rounded-lg">
          {error}
        </div>
      )}

      {locationWarning && (
        <div className="flex items-center gap-2 bg-amber-50 border border-amber-200 text-amber-700 text-sm px-4 py-3 rounded-lg">
          {locationWarning}
        </div>
      )}

      <section className="bg-white rounded-xl border border-slate-200/80 p-6 space-y-5">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-base font-semibold text-slate-900">Currencies</h2>
            <p className="text-sm text-slate-500 mt-0.5">
              Existing currencies are for viewing only. You can add new currencies.
            </p>
          </div>
        </div>

        <form
          onSubmit={handleCreateCurrency}
          className="grid grid-cols-1 md:grid-cols-4 gap-3"
        >
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            placeholder="Code"
            value={currencyForm.code}
            onChange={e =>
              setCurrencyForm(prev => ({ ...prev, code: e.target.value }))
            }
          />
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            placeholder="Name"
            value={currencyForm.name}
            onChange={e =>
              setCurrencyForm(prev => ({ ...prev, name: e.target.value }))
            }
          />
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            type="number"
            step="0.0001"
            placeholder="Exchange rate"
            value={currencyForm.exchangeRateToBase}
            onChange={e =>
              setCurrencyForm(prev => ({
                ...prev,
                exchangeRateToBase: e.target.value,
              }))
            }
          />
          <button
            type="submit"
            disabled={saving}
            className="text-sm font-medium bg-teal-600 text-white rounded-lg px-4 py-2.5 hover:bg-teal-700 disabled:opacity-50 transition-colors"
          >
            Add Currency
          </button>
        </form>

        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="text-left text-xs font-semibold text-slate-500 uppercase tracking-wider border-b border-slate-200">
                <th>Code</th>
                <th>Name</th>
                <th>Rate</th>
                <th>Active</th>
              </tr>
            </thead>
            <tbody>
              {currencies.map(currency => (
                <tr key={currency.id} className="hover:bg-slate-50/50 border-b border-slate-100">
                  <td className="p-2">{currency.code}</td>
                  <td className="p-2">{currency.name}</td>
                  <td className="p-2">{currency.exchangeRateToBase}</td>
                  <td className="p-2">{currency.isActive ? "Yes" : "No"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      <section className="bg-white rounded-xl border border-slate-200/80 p-6 space-y-5">
        <div>
          <h2 className="text-base font-semibold text-slate-900">Fee Configurations</h2>
          <p className="text-sm text-slate-500 mt-0.5">
            Existing fee configurations are for viewing only. You can add new fee configurations.
          </p>
        </div>

        <form
          onSubmit={handleCreateFee}
          className="grid grid-cols-1 md:grid-cols-5 gap-3"
        >
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            placeholder="Name"
            value={feeForm.name}
            onChange={e =>
              setFeeForm(prev => ({ ...prev, name: e.target.value }))
            }
          />
          <select
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={feeForm.type}
            onChange={e =>
              setFeeForm(prev => ({
                ...prev,
                type: Number(e.target.value) as FeeType,
              }))
            }
          >
            {Object.entries(FeeType)
              .filter(([, value]) => typeof value === "number")
              .map(([label, value]) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
          </select>
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            type="number"
            step="0.0001"
            placeholder="Percentage"
            value={feeForm.percentage}
            onChange={e =>
              setFeeForm(prev => ({ ...prev, percentage: e.target.value }))
            }
          />
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            type="date"
            value={feeForm.effectiveFrom}
            onChange={e =>
              setFeeForm(prev => ({ ...prev, effectiveFrom: e.target.value }))
            }
          />
          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            type="date"
            value={feeForm.effectiveTo}
            onChange={e =>
              setFeeForm(prev => ({ ...prev, effectiveTo: e.target.value }))
            }
          />
          <button
            type="submit"
            disabled={saving}
            className="md:col-span-5 text-sm font-medium bg-teal-600 text-white rounded-lg px-4 py-2.5 hover:bg-teal-700 disabled:opacity-50 transition-colors"
          >
            Add Fee Configuration
          </button>
        </form>

        <div className="space-y-3">
          {fees.map(fee => {
            const isEditing = editingFeeId === fee.id && feeDraft;

            return (
              <div
                key={fee.id}
                className="border border-slate-200 rounded-xl p-4 bg-slate-50/50 space-y-3"
              >
                {isEditing ? (
                  <div className="grid grid-cols-1 md:grid-cols-6 gap-3">
                    <input
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      value={feeDraft.name}
                      onChange={e =>
                        setFeeDraft(prev =>
                          prev ? { ...prev, name: e.target.value } : prev
                        )
                      }
                    />

                    <select
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      value={feeDraft.type}
                      onChange={e =>
                        setFeeDraft(prev =>
                          prev
                            ? {
                                ...prev,
                                type: Number(e.target.value) as FeeType,
                              }
                            : prev
                        )
                      }
                    >
                      {Object.entries(FeeType)
                        .filter(([, value]) => typeof value === "number")
                        .map(([label, value]) => (
                          <option key={value} value={value}>
                            {label}
                          </option>
                        ))}
                    </select>

                    <input
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      type="number"
                      step="0.0001"
                      value={feeDraft.percentage}
                      onChange={e =>
                        setFeeDraft(prev =>
                          prev ? { ...prev, percentage: e.target.value } : prev
                        )
                      }
                    />

                    <input
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      type="date"
                      value={feeDraft.effectiveFrom}
                      onChange={e =>
                        setFeeDraft(prev =>
                          prev ? { ...prev, effectiveFrom: e.target.value } : prev
                        )
                      }
                    />

                    <input
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      type="date"
                      value={feeDraft.effectiveTo}
                      onChange={e =>
                        setFeeDraft(prev =>
                          prev ? { ...prev, effectiveTo: e.target.value } : prev
                        )
                      }
                    />

                    <label className="flex items-center gap-2">
                      <input
                        type="checkbox"
                        checked={feeDraft.isActive}
                        onChange={e =>
                          setFeeDraft(prev =>
                            prev ? { ...prev, isActive: e.target.checked } : prev
                          )
                        }
                      />
                      Active
                    </label>

                    <div className="md:col-span-6 flex gap-2">
                      <button
                        type="button"
                        onClick={() => handleUpdateFee(fee.id)}
                        disabled={saving}
                        className="text-sm font-medium bg-teal-600 text-white rounded-lg px-4 py-2 hover:bg-teal-700 disabled:opacity-50 transition-colors"
                      >
                        Save
                      </button>
                      <button
                        type="button"
                        onClick={cancelFeeEdit}
                        className="text-sm font-medium border border-slate-200 text-slate-700 rounded-lg px-4 py-2 hover:bg-slate-50 transition-colors"
                      >
                        Cancel
                      </button>
                    </div>
                  </div>
                ) : (
                  <div className="flex items-start justify-between gap-4">
                    <div className="grid grid-cols-1 md:grid-cols-7 gap-3 flex-1">
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Name</div>
                        <div>{fee.name}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Type</div>
                        <div>{getFeeTypeLabel(fee.type)}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Risk Indicator</div>
                        <div>{getRiskIndicatorLabel(fee.riskIndicator)}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Percentage</div>
                        <div>{fee.percentage}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">From</div>
                        <div>{toDateInput(fee.effectiveFrom)}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">To</div>
                        <div>{fee.effectiveTo ? toDateInput(fee.effectiveTo) : "-"}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Active</div>
                        <div>{fee.isActive ? "Yes" : "No"}</div>
                      </div>
                    </div>

                    <button
                      type="button"
                      onClick={() => startFeeEdit(fee)}
                      className="text-sm font-medium text-slate-700 border border-slate-200 rounded-lg px-4 py-2 hover:bg-slate-50 transition-colors"
                    >
                      Edit
                    </button>
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </section>

      <section className="bg-white rounded-xl border border-slate-200/80 p-6 space-y-5">
        <h2 className="text-base font-semibold text-slate-900">Risk Factors</h2>

        <form
          onSubmit={handleCreateRisk}
          className="grid grid-cols-1 md:grid-cols-5 gap-3"
        >
          <select
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={riskForm.level}
            onChange={e =>
              handleRiskLevelChange(
                Number(e.target.value) as RiskFactorLevel,
                "create"
              )
            }
          >
            {Object.entries(RiskFactorLevel)
              .filter(([, value]) => typeof value === "number")
              .map(([label, value]) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
          </select>

          {renderRiskReferenceSelectors(riskForm, "create")}

          <input
            className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            type="number"
            step="0.0001"
            placeholder="Adjustment %"
            value={riskForm.adjustmentPercentage}
            onChange={e =>
              setRiskForm(prev => ({
                ...prev,
                adjustmentPercentage: e.target.value,
              }))
            }
          />

          <button
            type="submit"
            disabled={saving}
            className="text-sm font-medium bg-teal-600 text-white rounded-lg px-4 py-2.5 hover:bg-teal-700 disabled:opacity-50 transition-colors"
          >
            Add Risk Factor
          </button>
        </form>

        <div className="space-y-3">
          {riskFactors.map(risk => {
            const isEditing = editingRiskId === risk.id && riskDraft;

            return (
              <div
                key={risk.id}
                className="border border-slate-200 rounded-xl p-4 bg-slate-50/50 space-y-3"
              >
                {isEditing ? (
                  <div className="grid grid-cols-1 md:grid-cols-6 gap-3">
                    <select
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      value={riskDraft.level}
                      onChange={e =>
                        handleRiskLevelChange(
                          Number(e.target.value) as RiskFactorLevel,
                          "edit"
                        )
                      }
                    >
                      {Object.entries(RiskFactorLevel)
                        .filter(([, value]) => typeof value === "number")
                        .map(([label, value]) => (
                          <option key={value} value={value}>
                            {label}
                          </option>
                        ))}
                    </select>

                    {renderRiskReferenceSelectors(riskDraft, "edit")}

                    <input
                      className="text-sm border border-slate-200 rounded-lg px-3 py-2 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
                      type="number"
                      step="0.0001"
                      value={riskDraft.adjustmentPercentage}
                      onChange={e =>
                        setRiskDraft(prev =>
                          prev
                            ? { ...prev, adjustmentPercentage: e.target.value }
                            : prev
                        )
                      }
                    />

                    <label className="flex items-center gap-2">
                      <input
                        type="checkbox"
                        checked={riskDraft.isActive}
                        onChange={e =>
                          setRiskDraft(prev =>
                            prev ? { ...prev, isActive: e.target.checked } : prev
                          )
                        }
                      />
                      Active
                    </label>

                    <div className="md:col-span-6 flex gap-2">
                      <button
                        type="button"
                        onClick={() => handleUpdateRisk(risk.id)}
                        disabled={saving}
                        className="text-sm font-medium bg-teal-600 text-white rounded-lg px-4 py-2 hover:bg-teal-700 disabled:opacity-50 transition-colors"
                      >
                        Save
                      </button>
                      <button
                        type="button"
                        onClick={cancelRiskEdit}
                        className="text-sm font-medium border border-slate-200 text-slate-700 rounded-lg px-4 py-2 hover:bg-slate-50 transition-colors"
                      >
                        Cancel
                      </button>
                    </div>
                  </div>
                ) : (
                  <div className="flex items-start justify-between gap-4">
                    <div className="grid grid-cols-1 md:grid-cols-4 gap-3 flex-1">
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Level</div>
                        <div>{getRiskLevelLabel(risk.level)}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Reference</div>
                        <div>{getReferenceLabel(risk)}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Adjustment</div>
                        <div>{risk.adjustmentPercentage}</div>
                      </div>
                      <div>
                        <div className="text-xs text-slate-500 mb-0.5">Active</div>
                        <div>{risk.isActive ? "Yes" : "No"}</div>
                      </div>
                    </div>

                    <button
                      type="button"
                      onClick={() => startRiskEdit(risk)}
                      className="text-sm font-medium text-slate-700 border border-slate-200 rounded-lg px-4 py-2 hover:bg-slate-50 transition-colors"
                    >
                      Edit
                    </button>
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </section>
    </div>
  );
}

function toDateInput(value: string) {
  return value.slice(0, 10);
}

function normalizeFeeConfiguration(fee: ApiFeeConfiguration): FeeConfiguration {
  return {
    ...fee,
    type: parseFeeType(fee.type),
    riskIndicator: parseRiskIndicator(fee.riskIndicator),
  };
}

function normalizeRiskFactor(risk: ApiRiskFactor): RiskFactor {
  return {
    ...risk,
    level: parseRiskFactorLevel(risk.level),
  };
}

function parseFeeType(value: FeeType | string | number): FeeType {
  if (typeof value === "number") {
    return value as FeeType;
  }

  if (typeof value === "string") {
    const normalizedValue = feeTypeByApiValue[value];
    if (normalizedValue) {
      return normalizedValue;
    }

    const numericValue = Number(value);
    if (!Number.isNaN(numericValue)) {
      return numericValue as FeeType;
    }
  }

  return FeeType.BrokerCommission;
}

function parseRiskFactorLevel(
  value: RiskFactorLevel | string | number
): RiskFactorLevel {
  if (typeof value === "number") {
    return value as RiskFactorLevel;
  }

  if (typeof value === "string") {
    const normalizedValue = riskFactorLevelByApiValue[value];
    if (normalizedValue) {
      return normalizedValue;
    }

    const numericValue = Number(value);
    if (!Number.isNaN(numericValue)) {
      return numericValue as RiskFactorLevel;
    }
  }

  return RiskFactorLevel.Country;
}

function parseRiskIndicator(
  value?: RiskIndicator | string | number | null
): RiskIndicator | null {
  if (value == null) {
    return null;
  }

  if (typeof value === "number") {
    return value as RiskIndicator;
  }

  const normalizedValue = riskIndicatorByApiValue[value];
  if (normalizedValue) {
    return normalizedValue;
  }

  const numericValue = Number(value);
  return Number.isNaN(numericValue) ? null : (numericValue as RiskIndicator);
}

export default MetadataPage;