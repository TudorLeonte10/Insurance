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
  return (
    <div className="p-6 space-y-6 max-w-xl mx-auto">
      <h1 className="text-2xl font-bold">Create Policy</h1>

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

      <div className="bg-white p-6 rounded-xl shadow space-y-4">

        {/* CLIENT */}
        <select
          className="w-full border p-2 rounded"
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

        {/* BUILDING */}
        <select
          className="w-full border p-2 rounded"
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

        {/* CURRENCY */}
        <select
          className="w-full border p-2 rounded"
          value={form.currencyId}
          onChange={(e) => handleChange("currencyId", e.target.value)}
        >
          <option value="">Select currency</option>
          {currencies.map((c) => (
            <option key={c.id} value={c.id}>
              {c.code}
            </option>
          ))}
        </select>

        {/* PREMIUM */}
        <input
          type="number"
          placeholder="Base Premium"
          className="w-full border p-2 rounded"
          value={form.basePremium}
          onChange={(e) =>
            handleChange("basePremium", e.target.value)
          }
        />

        {/* DATES */}
        <input
          type="date"
          className="w-full border p-2 rounded"
          onChange={(e) =>
            handleChange("startDate", e.target.value)
          }
        />

        <input
          type="date"
          className="w-full border p-2 rounded"
          onChange={(e) =>
            handleChange("endDate", e.target.value)
          }
        />

        {/* SUBMIT */}
        <button
          onClick={handleSubmit}
          className="w-full bg-linear-to-r from-indigo-500 to-purple-600
                     text-white py-2 rounded-lg
                     hover:opacity-90 active:scale-95 transition"
        >
          Create Policy
        </button>
      </div>
    </div>
  );
}

export default CreatePolicyPage;