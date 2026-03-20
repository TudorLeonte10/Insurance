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

  return (
    <div className="p-8 max-w-3xl mx-auto space-y-6">

      <h1 className="text-3xl font-bold text-gray-800">
        Create Building
      </h1>

      {error && (
        <div className="bg-red-100 text-red-700 p-3 rounded">
          {error}
        </div>
      )}

      {success && (
        <div className="bg-green-100 text-green-700 p-3 rounded">
          {success}
        </div>
      )}

      <div className="bg-white shadow-lg rounded-xl p-6 space-y-6">

        {/* ADDRESS */}

        <div className="grid grid-cols-2 gap-4">

          <div>
            <label className="font-semibold text-sm">
              Street
            </label>

            <input
              className={`border rounded p-2 w-full ${
                errors.street ? "border-red-500 bg-red-50" : ""
              }`}
              value={form.street}
              onChange={(e) => handleChange("street", e.target.value)}
            />
          </div>

          <div>
            <label className="font-semibold text-sm">
              Number
            </label>

            <input
              className={`border rounded p-2 w-full ${
                errors.number ? "border-red-500 bg-red-50" : ""
              }`}
              value={form.number}
              onChange={(e) => handleChange("number", e.target.value)}
            />
          </div>

        </div>

        {/* LOCATION */}

        <div className="grid grid-cols-3 gap-4">

          <div>
            <label className="font-semibold text-sm">
              Country
            </label>

            <select
              className="border rounded p-2 w-full"
              value={form.countryId}
              onChange={(e) => handleCountryChange(e.target.value)}
            >
              <option value="">Select country</option>

              {countries.map(c => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}

            </select>
          </div>

          <div>
            <label className="font-semibold text-sm">
              County
            </label>

            <select
              className="border rounded p-2 w-full"
              value={form.countyId}
              onChange={(e) => handleCountyChange(e.target.value)}
            >
              <option value="">Select county</option>

              {counties.map(c => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}

            </select>
          </div>

          <div>
            <label className="font-semibold text-sm">
              City
            </label>

            <select
              className={`border rounded p-2 w-full ${
                errors.cityId ? "border-red-500 bg-red-50" : ""
              }`}
              value={form.cityId}
              onChange={(e) => handleChange("cityId", e.target.value)}
            >
              <option value="">Select city</option>

              {cities.map(c => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}

            </select>
          </div>

        </div>

        {/* BUILDING TYPE */}

        <div>
          <label className="font-semibold text-sm">
            Building Type
          </label>

          <select
            className="border rounded p-2 w-full"
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

        {/* NUMERIC VALUES */}

        <div className="grid grid-cols-3 gap-4">

          <input
            type="number"
            placeholder="Construction Year"
            className="border p-2 rounded"
            value={form.constructionYear}
            onChange={(e) => handleNumberChange("constructionYear", e.target.value)}
          />

          <input
            type="number"
            placeholder="Surface Area"
            className={`border p-2 rounded ${
              errors.surfaceArea ? "border-red-500 bg-red-50" : ""
            }`}
            value={form.surfaceArea}
            onChange={(e) => handleNumberChange("surfaceArea", e.target.value)}
          />

          <input
            type="number"
            placeholder="Floors"
            className={`border p-2 rounded ${
              errors.numberOfFloors ? "border-red-500 bg-red-50" : ""
            }`}
            value={form.numberOfFloors}
            onChange={(e) => handleNumberChange("numberOfFloors", e.target.value)}
          />

        </div>

        <input
          type="number"
          placeholder="Insured Value"
          className={`border p-2 rounded w-full ${
            errors.insuredValue ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.insuredValue}
          onChange={(e) => handleNumberChange("insuredValue", e.target.value)}
        />

        {/* RISK INDICATORS */}

        <div>

          <label className="font-semibold text-sm">
            Risk Indicators
          </label>

          <div className="grid grid-cols-2 gap-2 mt-2">

            {Object.values(RiskIndicator)
              .filter(v => typeof v === "number")
              .map(v => (

                <label key={v} className="flex gap-2">

                  <input
                    type="checkbox"
                    onChange={() => toggleRisk(v as RiskIndicator)}
                  />

                  {RiskIndicator[v as number]}

                </label>

              ))}

          </div>

        </div>

        <button
          onClick={handleSubmit}
          className="bg-[#00204a] text-white px-6 py-2 rounded-lg hover:opacity-90"
        >
          Create Building
        </button>

      </div>

    </div>
  );
}

export default CreateBuildingPage;