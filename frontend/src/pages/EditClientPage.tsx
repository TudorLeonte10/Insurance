import { useEffect, useState} from "react";
import { getClientById, updateClient } from "../api/clientApi";
import { useParams } from "react-router-dom";

function EditClientPage() {

  const { id } = useParams();

  const [loading, setLoading] = useState(true);

  const [form, setForm] = useState({
    name: "",
    identificationNumber: "",
    email: "",
    phoneNumber: "",
    address: ""
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

  useEffect(() => {
    const loadClient = async () => {
      try {
        const data = await getClientById(id!);

        setForm({
          name: data.name,
          identificationNumber: data.identificationNumber,
          email: data.email,
          phoneNumber: data.phoneNumber,
          address: data.address,
        });

      } catch {
        setError("Failed to load client.");
      } finally {
        setLoading(false);
      }
    };

    loadClient();
  }, [id]);

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

    try {

      await updateClient(id!, form);

      setSuccess("Client updated successfully.");

    } catch (err: any) {
      setError(err.response?.data || "Update failed.");
    }
  };

  if (loading) {
    return <div className="p-6">Loading...</div>;
  }

  return (
    <div className="p-8 max-w-2xl mx-auto space-y-6">

      <h1 className="text-3xl font-bold text-gray-800">
        Edit Client
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

        <input
          placeholder="Name"
          className={`border p-2 rounded w-full ${
            errors.name ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.name}
          onChange={(e) => handleChange("name", e.target.value)}
        />

        <input
          placeholder="Identification Number"
          className={`border p-2 rounded w-full ${
            errors.identificationNumber ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.identificationNumber}
          onChange={(e) => handleChange("identificationNumber", e.target.value)}
        />

        <input
          placeholder="Email"
          className={`border p-2 rounded w-full ${
            errors.email ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.email}
          onChange={(e) => handleChange("email", e.target.value)}
        />

        <input
          placeholder="Phone"
          className={`border p-2 rounded w-full ${
            errors.phoneNumber ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.phoneNumber}
          onChange={(e) => handleChange("phoneNumber", e.target.value)}
        />

        <input
          placeholder="Address"
          className={`border p-2 rounded w-full ${
            errors.address ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.address}
          onChange={(e) => handleChange("address", e.target.value)}
        />

        <button
          onClick={handleSubmit}
          className="bg-[#00204a] text-white px-6 py-2 rounded-lg w-full hover:opacity-90"
        >
          Update Client
        </button>

      </div>
    </div>
  );
}

export default EditClientPage;