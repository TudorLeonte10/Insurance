import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {createPolicy, getPolicyById} from "../api/policyApi";

import { getCurrencies } from "../api/metadaApi";
import { getBrokerClients } from "../api/brokerApi";
import { getBuildingsByClient } from "../api/buildingApi";



function CreatePolicyPage() {
  const navigate = useNavigate();

  const [clients, setClients] = useState<any[]>([]);
  const [buildings, setBuildings] = useState<any[]>([]);
  const [currencies, setCurrencies] = useState<any[]>([]);

  const [form, setForm] = useState({
    clientId: "",
    buildingId: "",
    currencyId: "",
    basePremium: 0,
    startDate: "",
    endDate: ""
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    getBrokerClients(1,100).then((res) => setClients(res.items));
    getCurrencies().then(res => setCurrencies(res.items));
  }, []);

  useEffect(() => {
    if (form.clientId) {
      getBuildingsByClient(form.clientId).then(setBuildings);
    } else {
      setBuildings([]);
    }
  }, [form.clientId]);

  const handleChange = (field: string, value: any) => {
    setForm((prev) => ({
      ...prev,
      [field]: value
    }));
  };

  const handleSubmit = async () => {
  try {
    setError("");
    setSuccess("");

    const result = await createPolicy({
      ...form,
      basePremium: Number(form.basePremium),
      startDate: new Date(form.startDate).toISOString(),
      endDate: new Date(form.endDate).toISOString()
    });

    const createdPolicy = await getPolicyById(result.id);

    if (createdPolicy.status === "draft") {
      setSuccess("Policy created and is ready for activation.");
    } else if (createdPolicy.status === "underReview") {
      setSuccess(
        "Policy created but requires review due to risk factors."
      );
    }

    setTimeout(() => {
      navigate("/broker/policies");
    }, 1500);

  } catch (err: any) {
    setError(
      err?.response?.data?.message ||
      "Failed to create policy"
    );
  }
};
  const selectClass =
    "w-full text-sm border border-slate-200 rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500 disabled:bg-slate-50 disabled:text-slate-400";

  const inputClass =
    "w-full text-sm border border-slate-200 rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500";

  return (
    <div className="max-w-lg mx-auto space-y-6">

      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
          Create Policy
        </h1>
        <p className="text-sm text-slate-500 mt-0.5">
          Issue a new insurance policy
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

      <div className="bg-white rounded-xl border border-slate-200/80 p-6 space-y-5">

        {/* CLIENT */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Client</label>
          <select
            className={selectClass}
            value={form.clientId}
            onChange={(e) => handleChange("clientId", e.target.value)}
          >
            <option value="">Select client</option>
            {clients.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>

        {/* BUILDING */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Building</label>
          <select
            className={selectClass}
            value={form.buildingId}
            onChange={(e) => handleChange("buildingId", e.target.value)}
            disabled={!form.clientId}
          >
            <option value="">Select building</option>
            {buildings.map((b) => (
              <option key={b.id} value={b.id}>
                {b.street} {b.number} - {b.city}
              </option>
            ))}
          </select>
        </div>

        {/* CURRENCY + PREMIUM */}
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">Currency</label>
            <select
              className={selectClass}
              value={form.currencyId}
              onChange={(e) => handleChange("currencyId", e.target.value)}
            >
              <option value="">Select</option>
              {currencies.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.code}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">Base Premium</label>
            <input
              type="number"
              placeholder="0.00"
              className={inputClass}
              value={form.basePremium}
              onChange={(e) =>
                handleChange("basePremium", e.target.value)
              }
            />
          </div>
        </div>

        {/* DATES */}
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">Start Date</label>
            <input
              type="date"
              className={inputClass}
              onChange={(e) =>
                handleChange("startDate", e.target.value)
              }
            />
          </div>
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">End Date</label>
            <input
              type="date"
              className={inputClass}
              onChange={(e) =>
                handleChange("endDate", e.target.value)
              }
            />
          </div>
        </div>

        {/* SUBMIT */}
        <button
          onClick={handleSubmit}
          className="w-full text-sm font-medium px-4 py-2.5 rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          Create Policy
        </button>
      </div>
    </div>
  );
}

export default CreatePolicyPage;