import { useEffect, useState} from "react";
import { getBrokerById, updateBroker } from "../api/brokerApi";
import { useParams } from "react-router-dom";

function EditBrokerPage() {

  const { id } = useParams();

  const [loading, setLoading] = useState(true);

  const [form, setForm] = useState({
    name: "",
    email: "",
    phone: "",
  });

  const [errors, setErrors] = useState({
    name: false,
    email: false,
    phone: false,
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    const loadBroker = async () => {
      try {
        const data = await getBrokerById(id!);

        setForm({
          name: data.name,
          email: data.email,
          phone: data.phone,
        });

      } catch {
        setError("Failed to load broker.");
      } finally {
        setLoading(false);
      }
    };

    loadBroker();
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

    try {

      await updateBroker(form, id!);

      setSuccess("Broker updated successfully.");

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
        Edit Broker
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
            errors.phone ? "border-red-500 bg-red-50" : ""
          }`}
          value={form.phone}
          onChange={(e) => handleChange("phone", e.target.value)}
        />

        <button
          onClick={handleSubmit}
          className="bg-[#00204a] text-white px-6 py-2 rounded-lg w-full hover:opacity-90"
        >
          Update Broker
        </button>

      </div>
    </div>
  );
}

export default EditBrokerPage;