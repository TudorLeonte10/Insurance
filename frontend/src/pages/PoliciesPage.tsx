import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getPolicies,
  activatePolicy,
  cancelPolicy
} from "../api/policyApi";

interface Policy {
  id: string;
  status: "draft" | "underReview" | "active" | "cancelled" | "expired";
  client: { id: string; name: string };
  building: {
    geography: { city: string; county: string };
  };
  currency: { code: string };
  finalPremium: number;
}

function BrokerPoliciesPage() {
  const [policies, setPolicies] = useState<Policy[]>([]);
  const [loading, setLoading] = useState(true);

  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const [filters, setFilters] = useState({
    clientId: "",
    status: "",
    startDateFrom: "",
    startDateTo: ""
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const navigate = useNavigate();

  const pageSize = 10;

  const fetchPolicies = async (pageNumber = page) => {
    try {
      setLoading(true);

      const data = await getPolicies({
        ...filters,
        pageNumber,
        pageSize
      });

      setPolicies(data.items);

      if (data.totalCount) {
        setTotalPages(Math.ceil(data.totalCount / pageSize));
      }
    } catch (err) {
      console.error(err);
      setError("Failed to load policies");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPolicies(1);
  }, []);

  const handleActivate = async (id: string) => {
    try {
      setError("");
      setSuccess("");

      await activatePolicy(id);

      setSuccess("Policy activated successfully");
      fetchPolicies(page);
    } catch (err: any) {
      const message =
        err?.response?.data?.message ||
        err?.response?.data ||
        "Only draft policies can be activated.";

      setError(message);
    }
  };

  const handleCancel = async (id: string) => {
    try {
      setError("");
      setSuccess("");

      await cancelPolicy(id);

      setSuccess("Policy cancelled successfully");
      fetchPolicies(page);
    } catch (err: any) {
      const message =
        err?.response?.data?.message ||
        err?.response?.data ||
        "Only active policies can be cancelled.";

      setError(message);
    }
  };

  const handleSearch = () => {
    setPage(1);
    fetchPolicies(1);
  };

  const goToPage = (newPage: number) => {
    setPage(newPage);
    fetchPolicies(newPage);
  };

  if (loading) return <div className="p-6">Loading...</div>;

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">Policies</h1>

      <div className="flex justify-between items-center">

        <button
            onClick={() => navigate("/broker/policies/create")}
            className="bg-linear-to-r from-indigo-500 to-purple-600
                    text-white px-4 py-2 rounded-lg
                    hover:opacity-90 active:scale-95 transition"
        >
            Create Policy
        </button>
       </div>

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

      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 bg-white p-4 rounded-xl shadow">
        <select
          className="border p-2 rounded"
          onChange={(e) =>
            setFilters({ ...filters, status: e.target.value })
          }
        >
          <option value="">All statuses</option>
          <option value="draft">Draft</option>
          <option value="underReview">Under Review</option>
          <option value="active">Active</option>
          <option value="cancelled">Cancelled</option>
        </select>

        <input
          type="date"
          className="border p-2 rounded"
          onChange={(e) =>
            setFilters({ ...filters, startDateFrom: e.target.value })
          }
        />

        <input
          type="date"
          className="border p-2 rounded"
          onChange={(e) =>
            setFilters({ ...filters, startDateTo: e.target.value })
          }
        />

        <button
          onClick={handleSearch}
          className="col-span-2 md:col-span-4
                     bg-linear-to-r from-indigo-500 to-purple-600
                     text-white py-2 rounded-lg
                     hover:opacity-90 transition duration-200
                     active:scale-95"
        >
          Search
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {policies.map((p) => (
          <div
            key={p.id}
            className="bg-white p-4 rounded-2xl shadow border space-y-2 hover:shadow-lg transition duration-200"
          >
            <div className="text-lg font-semibold">
              {p.client.name}
            </div>

            <div className="text-sm text-gray-500">
              {p.building.geography.city},{" "}
              {p.building.geography.county}
            </div>

            <div className="text-sm">
              {p.finalPremium} {p.currency.code}
            </div>

            <div>
              {p.status === "draft" && (
                <span className="bg-green-100 text-green-700 px-2 py-1 rounded text-xs">
                  Ready
                </span>
              )}

              {p.status === "underReview" && (
                <span className="bg-yellow-100 text-yellow-700 px-2 py-1 rounded text-xs">
                  Needs Review
                </span>
              )}

              {p.status === "active" && (
                <span className="bg-blue-100 text-blue-700 px-2 py-1 rounded text-xs">
                  Active
                </span>
              )}

              {p.status === "cancelled" && (
                <span className="bg-gray-200 text-gray-700 px-2 py-1 rounded text-xs">
                  Cancelled
                </span>
              )}

              {p.status === "expired" && (
                <span className="bg-orange-100 text-orange-700 px-2 py-1 rounded text-xs">
                    Expired
                </span>
                )}
            </div>

            <div className="flex gap-2 pt-2 flex-wrap">
              <button
                onClick={() => navigate(`/broker/policies/${p.id}`)}
                className="px-3 py-1.5 rounded-lg bg-gray-900 text-white hover:bg-gray-700 transition duration-200 active:scale-95"
              >
                View
              </button>

              {p.status === "draft" && (
                <button
                  onClick={() => handleActivate(p.id)}
                  className="px-3 py-1.5 rounded-lg bg-linear-to-r from-indigo-500 to-purple-600 text-white hover:opacity-90 transition duration-200 active:scale-95"
                >
                  Activate
                </button>
              )}

              {p.status !== "cancelled" && p.status !== "underReview" && (
                <button
                  onClick={() => handleCancel(p.id)}
                  className="px-3 py-1.5 rounded-lg bg-red-200 text-gray-800 hover:bg-red-300 transition duration-200 active:scale-95"
                >
                  Cancel
                </button>
              )}
            </div>
          </div>
        ))}
      </div>

      <div className="flex justify-center gap-2 pt-4">
        <button
          disabled={page === 1}
          onClick={() => goToPage(page - 1)}
          className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50"
        >
          Prev
        </button>

        <span className="px-3 py-1">
          {page} / {totalPages}
        </span>

        <button
          disabled={page === totalPages}
          onClick={() => goToPage(page + 1)}
          className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
}

export default BrokerPoliciesPage;