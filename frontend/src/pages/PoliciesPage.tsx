import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Plus, Eye, Power, XCircle, ChevronLeft, ChevronRight, Filter, MapPin } from "lucide-react";
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

const statusConfig: Record<string, { label: string; bg: string; text: string; dot: string }> = {
  draft:       { label: "Draft",        bg: "bg-amber-50",   text: "text-amber-700",   dot: "bg-amber-500" },
  underReview: { label: "Under Review", bg: "bg-orange-50",  text: "text-orange-700",  dot: "bg-orange-500" },
  active:      { label: "Active",       bg: "bg-emerald-50", text: "text-emerald-700", dot: "bg-emerald-500" },
  cancelled:   { label: "Cancelled",    bg: "bg-slate-100",  text: "text-slate-500",   dot: "bg-slate-400" },
  expired:     { label: "Expired",      bg: "bg-red-50",     text: "text-red-600",     dot: "bg-red-400" },
};

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

  const pageSize = 9;

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

  if (loading) {
    return (
      <div className="space-y-4">
        <div className="h-8 w-48 bg-slate-200 rounded animate-pulse" />
        <div className="h-16 bg-slate-200 rounded-xl animate-pulse" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {[1, 2, 3].map((i) => (
            <div key={i} className="h-44 bg-slate-200 rounded-xl animate-pulse" />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">

      {/* HEADER */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
            Policies
          </h1>
          <p className="text-sm text-slate-500 mt-0.5">
            Manage insurance policies for your clients
          </p>
        </div>

        <button
          onClick={() => navigate("/broker/policies/create")}
          className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          <Plus size={15} />
          New Policy
        </button>
      </div>

      {/* ALERTS */}
      {error && (
        <div className="flex items-center gap-3 bg-red-50 border border-red-200 text-red-700 text-sm px-4 py-3 rounded-lg">
          <XCircle size={16} className="shrink-0" />
          {error}
        </div>
      )}

      {success && (
        <div className="flex items-center gap-3 bg-emerald-50 border border-emerald-200 text-emerald-700 text-sm px-4 py-3 rounded-lg">
          <Power size={16} className="shrink-0" />
          {success}
        </div>
      )}

      {/* FILTERS */}
      <div className="bg-white rounded-xl border border-slate-200/80 p-4">
        <div className="flex items-center gap-2 mb-3">
          <Filter size={14} className="text-slate-400" />
          <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Filters</span>
        </div>
        <div className="grid grid-cols-1 sm:grid-cols-4 gap-3">
          <select
            className="text-sm border border-slate-200 p-2.5 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            value={filters.status}
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
            className="text-sm border border-slate-200 p-2.5 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            placeholder="From"
            onChange={(e) =>
              setFilters({ ...filters, startDateFrom: e.target.value })
            }
          />

          <input
            type="date"
            className="text-sm border border-slate-200 p-2.5 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
            placeholder="To"
            onChange={(e) =>
              setFilters({ ...filters, startDateTo: e.target.value })
            }
          />

          <button
            onClick={handleSearch}
            className="text-sm font-medium px-4 py-2.5 rounded-lg bg-slate-900 text-white hover:bg-slate-800 transition-colors"
          >
            Apply
          </button>
        </div>
      </div>

      {/* POLICY CARDS */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {policies.map((p) => {
          const status = statusConfig[p.status] || statusConfig.draft;
          return (
            <div
              key={p.id}
              className="bg-white rounded-xl border border-slate-200/80 p-5 flex flex-col gap-4 hover:border-slate-300 transition-colors"
            >
              {/* TOP: client + status */}
              <div className="flex items-start justify-between gap-3">
                <div className="min-w-0">
                  <p className="font-medium text-slate-900 truncate">
                    {p.client.name}
                  </p>
                  <div className="flex items-center gap-1 mt-1 text-xs text-slate-500">
                    <MapPin size={12} className="shrink-0" />
                    <span className="truncate">
                      {p.building.geography.city}, {p.building.geography.county}
                    </span>
                  </div>
                </div>
                <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium whitespace-nowrap ${status.bg} ${status.text}`}>
                  <span className={`w-1.5 h-1.5 rounded-full ${status.dot}`} />
                  {status.label}
                </span>
              </div>

              {/* PREMIUM */}
              <div className="flex items-baseline gap-1">
                <span className="text-xl font-semibold text-slate-900">
                  {p.finalPremium.toLocaleString()}
                </span>
                <span className="text-sm text-slate-500">
                  {p.currency.code}
                </span>
              </div>

              {/* ACTIONS */}
              <div className="flex gap-2 pt-1 border-t border-slate-100">
                <button
                  onClick={() => navigate(`/broker/policies/${p.id}`)}
                  className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium border border-slate-200 text-slate-700 hover:bg-slate-50 transition-colors"
                >
                  <Eye size={13} />
                  View
                </button>

                {p.status === "draft" && (
                  <button
                    onClick={() => handleActivate(p.id)}
                    className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium bg-teal-600 text-white hover:bg-teal-700 transition-colors"
                  >
                    <Power size={13} />
                    Activate
                  </button>
                )}

                {p.status !== "cancelled" && p.status !== "underReview" && (
                  <button
                    onClick={() => handleCancel(p.id)}
                    className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium text-red-600 border border-red-200 hover:bg-red-50 transition-colors"
                  >
                    <XCircle size={13} />
                    Cancel
                  </button>
                )}
              </div>
            </div>
          );
        })}
      </div>

      {policies.length === 0 && (
        <div className="text-center py-16">
          <p className="text-sm text-slate-400">No policies found</p>
        </div>
      )}

      {/* PAGINATION */}
      <div className="flex items-center justify-between pt-2">
        <span className="text-sm text-slate-500">
          Page <span className="font-medium text-slate-700">{page}</span> of{" "}
          <span className="font-medium text-slate-700">{totalPages}</span>
        </span>

        <div className="flex gap-1.5 items-center">
          <button
            disabled={page === 1}
            onClick={() => goToPage(page - 1)}
            className="p-1.5 rounded-md border border-slate-200 bg-white hover:bg-slate-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
          >
            <ChevronLeft size={16} />
          </button>

          <span className="text-sm font-medium text-slate-700 px-2">
            {page}
          </span>

          <button
            disabled={page === totalPages}
            onClick={() => goToPage(page + 1)}
            className="p-1.5 rounded-md border border-slate-200 bg-white hover:bg-slate-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
          >
            <ChevronRight size={16} />
          </button>
        </div>
      </div>
    </div>
  );
}

export default BrokerPoliciesPage;