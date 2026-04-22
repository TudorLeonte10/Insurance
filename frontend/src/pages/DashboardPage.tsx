import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Users, FileText, ShieldCheck, TrendingUp, ArrowRight, BarChart3, Database } from "lucide-react";
import { useAuth } from "../auth/AuthContext";
import { getAllBrokers } from "../api/brokerApi";
import { getPolicySummary } from "../api/policyReportsApi";
import BrokerPoliciesMap from "../components/BrokerPoliciesMap";

function DashboardPage() {
  const { username } = useAuth();
  const navigate = useNavigate();

  const [brokerCount, setBrokerCount] = useState<number | null>(null);
  const [totalPolicies, setTotalPolicies] = useState<number | null>(null);
  const [activePolicies, setActivePolicies] = useState<number | null>(null);
  const [underReview, setUnderReview] = useState<number | null>(null);

  useEffect(() => {
    getAllBrokers(1, 1).then((res) => setBrokerCount(res.totalCount)).catch(() => {});
    getPolicySummary({})
      .then((s) => {
        setTotalPolicies(s.totalPolicies ?? 0);
        setActivePolicies(s.activePolicies ?? 0);
        setUnderReview(s.underReviewPolicies ?? 0);
      })
      .catch(() => {});
  }, []);

  const stats = [
    {
      label: "Total Brokers",
      value: brokerCount,
      icon: <Users size={18} />,
      color: "text-slate-600",
      bg: "bg-slate-100",
    },
    {
      label: "Total Policies",
      value: totalPolicies,
      icon: <FileText size={18} />,
      color: "text-teal-700",
      bg: "bg-teal-50",
    },
    {
      label: "Active Policies",
      value: activePolicies,
      icon: <TrendingUp size={18} />,
      color: "text-emerald-700",
      bg: "bg-emerald-50",
    },
    {
      label: "Under Review",
      value: underReview,
      icon: <ShieldCheck size={18} />,
      color: "text-amber-700",
      bg: "bg-amber-50",
    },
  ];

  const quickLinks = [
    {
      label: "Manage Brokers",
      description: "Create, edit and monitor brokers",
      icon: <Users size={16} />,
      path: "/admin/brokers",
    },
    {
      label: "Review Policies",
      description: "Approve or reject policies pending review",
      icon: <ShieldCheck size={16} />,
      path: "/admin/review-policies",
    },
    {
      label: "Reports",
      description: "Analyze policies across the platform",
      icon: <BarChart3 size={16} />,
      path: "/admin/reports",
    },
    {
      label: "Metadata",
      description: "Configure currencies, fees and risk factors",
      icon: <Database size={16} />,
      path: "/admin/metadata",
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
            {username || "Administrator"}
          </h1>
        </div>

        <div className="flex gap-2">
          <button
            onClick={() => navigate("/admin/brokers/create")}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg border border-slate-200 bg-white text-slate-700 hover:bg-slate-50 transition-colors"
          >
            <Users size={15} />
            New Broker
          </button>
          <button
            onClick={() => navigate("/admin/reports")}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
          >
            <BarChart3 size={15} />
            View Reports
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
        {quickLinks.map((link) => (
          <button
            key={link.path}
            onClick={() => navigate(link.path)}
            className="group flex items-center justify-between bg-white rounded-xl border border-slate-200/80 p-5 hover:border-slate-300 transition-colors text-left"
          >
            <div className="flex items-center gap-3">
              <span className="bg-slate-100 text-slate-600 p-2 rounded-lg group-hover:bg-teal-50 group-hover:text-teal-700 transition-colors">
                {link.icon}
              </span>
              <div>
                <p className="text-sm font-medium text-slate-900">{link.label}</p>
                <p className="text-xs text-slate-500 mt-0.5">{link.description}</p>
              </div>
            </div>
            <ArrowRight size={16} className="text-slate-400 group-hover:text-teal-600 group-hover:translate-x-0.5 transition-all" />
          </button>
        ))}
      </div>

      {/* MAP */}
      <BrokerPoliciesMap />
    </div>
  );
}

export default DashboardPage;