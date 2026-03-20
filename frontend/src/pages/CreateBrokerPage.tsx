import { useState } from "react";
import { createBroker } from "../api/brokerApi";

function CreateBrokerPage() {

  const [form, setForm] = useState({
    brokerCode: "",
    name: "",
    email: "",
    phone: "",
    isActive: true
  });

  const [errors, setErrors] = useState({
    brokerCode: false,
    name: false,
    email: false,
    phone: false,
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
      brokerCode: !form.brokerCode,
      email: !form.email,
      phone: !form.phone
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
    await createBroker(form);
    setSuccess("Broker created successfully!");
    setForm({
      brokerCode: "",
      name: "",
      email: "",
      phone: "",
      isActive: true
    });
    } catch(err :any) {
      setError("Failed to create broker.");
      
      if(err.response?.data) {
        setError(err.response.data);
    }   
      else 
        setError("Broker could not be created.");
    }
  };

  return (
    <div className="p-8 max-w-2xl mx-auto space-y-6">

      <h1 className="text-3xl font-bold text-gray-800">
        Create Broker
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

        {/* BROKER CODE */}
        <input
          placeholder="Broker Code"
          className={`border p-2 rounded w-full ${
            errors.brokerCode ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.brokerCode}
          onChange={(e) => handleChange("brokerCode", e.target.value)}
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
            errors.phone ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.phone}
          onChange={(e) => handleChange("phone", e.target.value)}
        />

        <button
          onClick={handleSubmit}
          className="bg-[#00204a] text-white px-6 py-2 rounded-lg hover:opacity-90 w-full"
        >
          Create Broker
        </button>

      </div>
    </div>
  );
}

export default CreateBrokerPage;