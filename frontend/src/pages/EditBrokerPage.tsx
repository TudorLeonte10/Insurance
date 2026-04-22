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

  const inputClass = (hasError: boolean) =>
    `w-full text-sm border rounded-lg px-4 py-2.5 bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500 ${
      hasError ? "border-red-300 bg-red-50/50" : "border-slate-200"
    }`;

  if (loading) {
    return (
      <div className="max-w-lg mx-auto space-y-4">
        <div className="h-8 w-48 bg-slate-200 rounded animate-pulse" />
        <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
      </div>
    );
  }

  return (
    <div className="max-w-lg mx-auto space-y-6">

      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
          Edit Broker
        </h1>
        <p className="text-sm text-slate-500 mt-0.5">
          Update broker information
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

        <div>
          <label className="block text-xs font-medium text-slate-600 mb-1.5">Name</label>
          <input
            placeholder="Full name"
            className={inputClass(errors.name)}
            value={form.name}
            onChange={(e) => handleChange("name", e.target.value)}
          />
        </div>

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
              className={inputClass(errors.phone)}
              value={form.phone}
              onChange={(e) => handleChange("phone", e.target.value)}
            />
          </div>
        </div>

        <button
          onClick={handleSubmit}
          className="w-full text-sm font-medium px-4 py-2.5 rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          Update Broker
        </button>

      </div>
    </div>
  );
}

export default EditBrokerPage;