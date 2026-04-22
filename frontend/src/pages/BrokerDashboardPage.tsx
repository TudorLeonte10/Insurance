import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Users, FileText, Plus, ArrowRight } from "lucide-react";
import { useAuth } from "../auth/AuthContext";
import { getBrokerClients } from "../api/brokerApi";
import { getPolicies } from "../api/policyApi";
import BrokerPoliciesMap from "../components/BrokerPoliciesMap";

function BrokerDashboardPage() {
  const { username } = useAuth();
  const navigate = useNavigate();

  const [clientCount, setClientCount] = useState<number | null>(null);
  const [policyCount, setPolicyCount] = useState<number | null>(null);
  const [draftCount, setDraftCount] = useState<number | null>(null);
  const [activeCount, setActiveCount] = useState<number | null>(null);

  useEffect(() => {
    getBrokerClients(1, 1).then((res) => setClientCount(res.totalCount)).catch(() => {});
    getPolicies({ pageNumber: 1, pageSize: 1 }).then((res) => setPolicyCount(res.totalCount)).catch(() => {});
    getPolicies({ pageNumber: 1, pageSize: 1, status: "draft" }).then((res) => setDraftCount(res.totalCount)).catch(() => {});
    getPolicies({ pageNumber: 1, pageSize: 1, status: "active" }).then((res) => setActiveCount(res.totalCount)).catch(() => {});
  }, []);

  const stats = [
    {
      label: "Total Clients",
      value: clientCount,
      icon: <Users size={18} />,
      color: "text-slate-600",
      bg: "bg-slate-100",
    },
    {
      label: "Total Policies",
      value: policyCount,
      icon: <FileText size={18} />,
      color: "text-teal-700",
      bg: "bg-teal-50",
    },
    {
      label: "Active Policies",
      value: activeCount,
      icon: <FileText size={18} />,
      color: "text-emerald-700",
      bg: "bg-emerald-50",
    },
    {
      label: "Draft Policies",
      value: draftCount,
      icon: <FileText size={18} />,
      color: "text-amber-700",
      bg: "bg-amber-50",
    },
  ];

  return (
    <div className="space-y-8">

      {/* HEADER */}
      <div className="flex flex-col sm:flex-row sm:items-end sm:justify-between gap-4">
        <div>
          <p className="text-sm text-slate-500 mb-1">
            Welcome back,
          </p>
          <h1 className="text-2xl font-semibold text-slate-900 tracking-tight">
            {username || "Broker"}
          </h1>
        </div>

        <div className="flex gap-2">
          <button
            onClick={() => navigate("/broker/clients/create")}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg border border-slate-200 bg-white text-slate-700 hover:bg-slate-50 transition-colors"
          >
            <Plus size={15} />
            New Client
          </button>
          <button
            onClick={() => navigate("/broker/policies/create")}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
          >
            <Plus size={15} />
            New Policy
          </button>
        </div>
      </div>

      {/* STATS GRID */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((stat) => (
          <div
            key={stat.label}
            className="bg-white rounded-xl border border-slate-200/80 p-5 flex flex-col gap-3"
          >
            <div className="flex items-center justify-between">
              <span className="text-xs font-medium text-slate-500 uppercase tracking-wide">
                {stat.label}
              </span>
              <span className={`${stat.bg} ${stat.color} p-1.5 rounded-lg`}>
                {stat.icon}
              </span>
            </div>
            <span className="text-2xl font-semibold text-slate-900">
              {stat.value !== null ? stat.value : "--"}
            </span>
          </div>
        ))}
      </div>

      {/* QUICK LINKS */}
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        <button
          onClick={() => navigate("/broker/clients")}
          className="group flex items-center justify-between bg-white rounded-xl border border-slate-200/80 p-5 hover:border-slate-300 transition-colors text-left"
        >
          <div>
            <p className="text-sm font-medium text-slate-900">Manage Clients</p>
            <p className="text-xs text-slate-500 mt-0.5">View and edit your client portfolio</p>
          </div>
          <ArrowRight size={16} className="text-slate-400 group-hover:text-teal-600 group-hover:translate-x-0.5 transition-all" />
        </button>
        <button
          onClick={() => navigate("/broker/policies")}
          className="group flex items-center justify-between bg-white rounded-xl border border-slate-200/80 p-5 hover:border-slate-300 transition-colors text-left"
        >
          <div>
            <p className="text-sm font-medium text-slate-900">Manage Policies</p>
            <p className="text-xs text-slate-500 mt-0.5">Review, activate, or cancel policies</p>
          </div>
          <ArrowRight size={16} className="text-slate-400 group-hover:text-teal-600 group-hover:translate-x-0.5 transition-all" />
        </button>
      </div>

      {/* MAP */}
      <BrokerPoliciesMap />
    </div>
  );
}

export default BrokerDashboardPage;
