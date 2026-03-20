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

  return (
    <div className="p-8 max-w-2xl mx-auto space-y-6">

      <h1 className="text-3xl font-bold text-gray-800">
        Create Client
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

        {/* NAME */}
        <input
          placeholder="Name"
          className={`border p-2 rounded w-full ${
            errors.name ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.name}
          onChange={(e) => handleChange("name", e.target.value)}
        />

        {/* IDENTIFICATION */}
        <input
          placeholder="Identification Number"
          className={`border p-2 rounded w-full ${
            errors.identificationNumber ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.identificationNumber}
          onChange={(e) => handleChange("identificationNumber", e.target.value)}
        />

        {/* EMAIL */}
        <input
          placeholder="Email"
          className={`border p-2 rounded w-full ${
            errors.email ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.email}
          onChange={(e) => handleChange("email", e.target.value)}
        />

        {/* PHONE */}
        <input
          placeholder="Phone Number"
          className={`border p-2 rounded w-full ${
            errors.phoneNumber ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.phoneNumber}
          onChange={(e) => handleChange("phoneNumber", e.target.value)}
        />

        {/* ADDRESS */}
        <input
          placeholder="Address"
          className={`border p-2 rounded w-full ${
            errors.address ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.address}
          onChange={(e) => handleChange("address", e.target.value)}
        />

        {/* TYPE */}
        <select
          className="border p-2 rounded w-full"
          value={form.type}
          onChange={(e) => handleChange("type", Number(e.target.value))}
        >
          <option value={ClientType.Individual}>Individual</option>
          <option value={ClientType.Company}>Company</option>
        </select>

        <button
          onClick={handleSubmit}
          className="bg-[#00204a] text-white px-6 py-2 rounded-lg hover:opacity-90 w-full"
        >
          Create Client
        </button>

      </div>
    </div>
  );
}

export default CreateClientPage;