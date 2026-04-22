import { useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { createBuilding } from "../api/buildingApi";
import { BuildingType, RiskIndicator } from "../types/buildings";
import { getCountries, getCounties, getCities } from "../api/locationApi";
import type { Country, County, City } from "../api/locationApi";

function CreateBuildingPage() {

  const { id } = useParams();

  const [countries, setCountries] = useState<Country[]>([]);
  const [counties, setCounties] = useState<County[]>([]);
  const [cities, setCities] = useState<City[]>([]);

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const [errors, setErrors] = useState({
    street: false,
    number: false,
    cityId: false,
    surfaceArea: false,
    numberOfFloors: false,
    insuredValue: false
  });

  const [form, setForm] = useState({
    street: "",
    number: "",
    countryId: "",
    countyId: "",
    cityId: "",
    constructionYear: 2000,
    type: BuildingType.Residential,
    surfaceArea: "",
    numberOfFloors: "",
    insuredValue: "",
    riskIndicators: [] as RiskIndicator[]
  });

  useEffect(() => {
    const loadCountries = async () => {
      try {
        const data = await getCountries();
        setCountries(data);
      } catch {
        setError("Failed to load countries.");
      }
    };

    loadCountries();
  }, []);

  const handleCountryChange = async (countryId: string) => {

    setForm(prev => ({
      ...prev,
      countryId,
      countyId: "",
      cityId: ""
    }));

    try {
      const data = await getCounties(countryId);
      setCounties(data);
      setCities([]);
    } catch {
      setError("Failed to load counties.");
    }
  };

  const handleCountyChange = async (countyId: string) => {

    setForm(prev => ({
      ...prev,
      countyId,
      cityId: ""
    }));

    try {
      const data = await getCities(countyId);
      setCities(data);
    } catch {
      setError("Failed to load cities.");
    }
  };

  const handleChange = (field: string, value: any) => {
    setForm(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleNumberChange = (field: string, value: string) => {

    const clean = value.replace(/^0+(?=\d)/, "");

    setForm(prev => ({
      ...prev,
      [field]: clean
    }));
  };

  const toggleRisk = (risk: RiskIndicator) => {

    setForm(prev => {

      const exists = prev.riskIndicators.includes(risk);

      return {
        ...prev,
        riskIndicators: exists
          ? prev.riskIndicators.filter(r => r !== risk)
          : [...prev.riskIndicators, risk]
      };
    });
  };

  const validateForm = () => {

    const newErrors = {
      street: !form.street,
      number: !form.number,
      cityId: !form.cityId,
      surfaceArea: !form.surfaceArea,
      numberOfFloors: !form.numberOfFloors,
      insuredValue: !form.insuredValue
    };

    setErrors(newErrors);

    return !Object.values(newErrors).some(e => e);
  };

  const handleSubmit = async () => {

    setError("");
    setSuccess("");

    if (!validateForm()) {
      setError("Please complete the required fields.");
      return;
    }

    try {

      await createBuilding(id!, {
        street: form.street,
        number: form.number,
        cityId: form.cityId,
        constructionYear: Number(form.constructionYear),
        type: form.type,
        surfaceArea: Number(form.surfaceArea),
        numberOfFloors: Number(form.numberOfFloors),
        insuredValue: Number(form.insuredValue),
        riskIndicators: form.riskIndicators
      });

      setSuccess("Building created successfully.");

     
    } catch (err: any) {

      if (err.response?.data) {
        setError(err.response.data);
      } else {
        setError("Building could not be created.");
      }
    }
  };

  const inputClass = (hasError: boolean) =>
    `w-full text-sm border rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500 ${
      hasError ? "border-red-300 bg-red-50/50" : "border-slate-200"
    }`;

  const selectClass = (hasError = false) =>
    `w-full text-sm border rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500 ${
      hasError ? "border-red-300 bg-red-50/50" : "border-slate-200"
    }`;

  return (
    <div className="max-w-2xl mx-auto space-y-6">

      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
          Create Building
        </h1>
        <p className="text-sm text-slate-500 mt-0.5">
          Register a new building for this client
        </p>
      </div>

      {error && (
        <div className="flex items-center gap-2 bg-red-50 border border-red-200 text-red-700 text-sm px-4 py-3 rounded-lg">
          {error}
        </div>
      )}

      {success && (
        <div className="flex items-center gap-2 bg-emerald-50 border border-emerald-200 text-emerald-700 text-sm px-4 py-3 rounded-lg">
          {success}
        </div>
      )}

      <div className="bg-white rounded-xl border border-slate-200/80 p-6 space-y-6">

        {/* ADDRESS */}
        <div>
          <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">Address</p>
          <div className="grid grid-cols-3 gap-4">
            <div className="col-span-2">
              <label className="block text-xs font-medium text-slate-600 mb-1.5">Street</label>
              <input
                className={inputClass(errors.street)}
                value={form.street}
                onChange={(e) => handleChange("street", e.target.value)}
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">Number</label>
              <input
                className={inputClass(errors.number)}
                value={form.number}
                onChange={(e) => handleChange("number", e.target.value)}
              />
            </div>
          </div>
        </div>

        {/* LOCATION */}
        <div>
          <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">Location</p>
          <div className="grid grid-cols-3 gap-4">
            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">Country</label>
              <select
                className={selectClass()}
                value={form.countryId}
                onChange={(e) => handleCountryChange(e.target.value)}
              >
                <option value="">Select</option>
                {countries.map(c => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">County</label>
              <select
                className={selectClass()}
                value={form.countyId}
                onChange={(e) => handleCountyChange(e.target.value)}
              >
                <option value="">Select</option>
                {counties.map(c => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">City</label>
              <select
                className={selectClass(errors.cityId)}
                value={form.cityId}
                onChange={(e) => handleChange("cityId", e.target.value)}
              >
                <option value="">Select</option>
                {cities.map(c => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>
          </div>
        </div>

        {/* BUILDING DETAILS */}
        <div>
          <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">Building Details</p>

          <div className="space-y-4">
            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">Building Type</label>
              <select
                className={selectClass()}
                value={form.type}
                onChange={(e) => handleChange("type", Number(e.target.value))}
              >
                {Object.values(BuildingType)
                  .filter(v => typeof v === "number")
                  .map(v => (
                    <option key={v} value={v}>
                      {BuildingType[v as number]}
                    </option>
                  ))}
              </select>
            </div>

            <div className="grid grid-cols-3 gap-4">
              <div>
                <label className="block text-xs font-medium text-slate-600 mb-1.5">Construction Year</label>
                <input
                  type="number"
                  className={inputClass(false)}
                  value={form.constructionYear}
                  onChange={(e) => handleNumberChange("constructionYear", e.target.value)}
                />
              </div>
              <div>
                <label className="block text-xs font-medium text-slate-600 mb-1.5">Surface Area (m2)</label>
                <input
                  type="number"
                  className={inputClass(errors.surfaceArea)}
                  value={form.surfaceArea}
                  onChange={(e) => handleNumberChange("surfaceArea", e.target.value)}
                />
              </div>
              <div>
                <label className="block text-xs font-medium text-slate-600 mb-1.5">Floors</label>
                <input
                  type="number"
                  className={inputClass(errors.numberOfFloors)}
                  value={form.numberOfFloors}
                  onChange={(e) => handleNumberChange("numberOfFloors", e.target.value)}
                />
              </div>
            </div>

            <div>
              <label className="block text-xs font-medium text-slate-600 mb-1.5">Insured Value</label>
              <input
                type="number"
                className={inputClass(errors.insuredValue)}
                value={form.insuredValue}
                onChange={(e) => handleNumberChange("insuredValue", e.target.value)}
              />
            </div>
          </div>
        </div>

        {/* RISK INDICATORS */}
        <div>
          <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3">Risk Indicators</p>
          <div className="grid grid-cols-2 gap-2">
            {Object.values(RiskIndicator)
              .filter(v => typeof v === "number")
              .map(v => (
                <label
                  key={v}
                  className="flex items-center gap-2.5 px-3 py-2.5 rounded-lg border border-slate-200 hover:bg-slate-50 cursor-pointer transition-colors text-sm text-slate-700"
                >
                  <input
                    type="checkbox"
                    className="rounded border-slate-300 text-teal-600 focus:ring-teal-500"
                    onChange={() => toggleRisk(v as RiskIndicator)}
                  />
                  {RiskIndicator[v as number]}
                </label>
              ))}
          </div>
        </div>

        <button
          onClick={handleSubmit}
          className="w-full text-sm font-medium px-4 py-2.5 rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          Create Building
        </button>

      </div>

    </div>
  );
}

export default CreateBuildingPage;