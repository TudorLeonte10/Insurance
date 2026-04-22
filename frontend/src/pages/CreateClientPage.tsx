import { useState } from "react";
import { createClient } from "../api/clientApi";
import { ClientType } from "../types/clients";

function CreateClientPage() {

  const [form, setForm] = useState({
    name: "",
    identificationNumber: "",
    email: "",
    phoneNumber: "",
    address: "",
    type: ClientType.Individual
  });

  const [errors, setErrors] = useState({
    name: false,
    identificationNumber: false,
    email: false,
    phoneNumber: false,
    address: false
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleChange = (field: string, value: any) => {
    setForm(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const validateForm = () => {

    const newErrors = {
      name: !form.name,
      identificationNumber: !form.identificationNumber,
      email: !form.email,
      phoneNumber: !form.phoneNumber,
      address: !form.address
    };

    setErrors(newErrors);

    return !Object.values(newErrors).some(e => e);
  };

  const handleSubmit = async () => {

    setError("");
    setSuccess("");

    if (!validateForm()) {
      setError("Please complete all required fields.");
      return;
    }

   try{
    await createClient(form);
    setSuccess("Client created successfully!");
    setForm({
      name: "",
      identificationNumber: "",
      email: "",
      phoneNumber: "",
      address: "",
      type: ClientType.Individual
    });
    } catch(err :any) {
      setError("Failed to create client.");
      
      if(err.response?.data) {
        setError(err.response.data);
    }   
      else 
        setError("Client could not be created.");
    }
  };

  const inputClass = (hasError: boolean) =>
    `w-full text-sm border rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500 ${
      hasError ? "border-red-300 bg-red-50/50" : "border-slate-200"
    }`;

  return (
    <div className="max-w-lg mx-auto space-y-6">

      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
          Create Client
        </h1>
        <p className="text-sm text-slate-500 mt-0.5">
          Add a new client to your portfolio
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

        {/* NAME */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Name</label>
          <input
            placeholder="Full name"
            className={inputClass(errors.name)}
            value={form.name}
            onChange={(e) => handleChange("name", e.target.value)}
          />
        </div>

        {/* IDENTIFICATION */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Identification Number</label>
          <input
            placeholder="CNP or CUI"
            className={inputClass(errors.identificationNumber)}
            value={form.identificationNumber}
            onChange={(e) => handleChange("identificationNumber", e.target.value)}
          />
        </div>

        {/* EMAIL + PHONE */}
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">Email</label>
            <input
              placeholder="email@example.com"
              className={inputClass(errors.email)}
              value={form.email}
              onChange={(e) => handleChange("email", e.target.value)}
            />
          </div>
          <div>
            <label className="block text-xs font-medium text-slate-600 mb-1.5">Phone</label>
            <input
              placeholder="+40..."
              className={inputClass(errors.phoneNumber)}
              value={form.phoneNumber}
              onChange={(e) => handleChange("phoneNumber", e.target.value)}
            />
          </div>
        </div>

        {/* ADDRESS */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Address</label>
          <input
            placeholder="Street, city, county"
            className={inputClass(errors.address)}
            value={form.address}
            onChange={(e) => handleChange("address", e.target.value)}
          />
        </div>

        {/* TYPE */}
        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Client Type</label>
          <select
            className="w-full text-sm border border-slate-200 rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={form.type}
            onChange={(e) => handleChange("type", Number(e.target.value))}
          >
            <option value={ClientType.Individual}>Individual</option>
            <option value={ClientType.Company}>Company</option>
          </select>
        </div>

        <button
          onClick={handleSubmit}
          className="w-full text-sm font-medium px-4 py-2.5 rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          Create Client
        </button>

      </div>
    </div>
  );
}

export default CreateClientPage;