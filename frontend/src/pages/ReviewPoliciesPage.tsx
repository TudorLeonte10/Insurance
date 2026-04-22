import { useEffect, useState } from "react";
import {
  Check,
  X,
  ChevronDown,
  ChevronUp,
  MapPin,
  User,
  Building2,
  Briefcase,
  Calendar,
  DollarSign,
  ShieldCheck,
} from "lucide-react";
import apiClient from "../api/apiClient";
import { getBrokerById } from "../api/brokerApi";

interface Policy {
  id: string;
  brokerId: string;
  policyNumber: string;
  status: string;
  finalPremium: number;
  basePremium: number;
  startDate: string;
  endDate: string;
  client: {
    name: string;
    identificationNumber: string;
  };
  building: {
    street: string;
    number: string;
    insuredValue: number;
    geography: {
      city: string;
      county: string;
      country: string;
    };
  };
  currency: {
    code: string;
    name: string;
  };
}

interface PolicyResponse {
  items: Policy[];
}

function AdminReviewPoliciesPage() {
  const [policies, setPolicies] = useState<Policy[]>([]);
  const [loading, setLoading] = useState(true);
  const [expandedId, setExpandedId] = useState<string | null>(null);
  const [brokers, setBrokers] = useState<Record<string, any>>({});

  const fetchPolicies = async () => {
    try {
      const res = await apiClient.get<PolicyResponse>("/admin/policy-review");
      setPolicies(res.data.items);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPolicies();
  }, []);

  const handleApprove = async (id: string) => {
    await apiClient.post(`/admin/policy-review/${id}/approve`);
    setPolicies((prev) => prev.filter((p) => p.id !== id));
  };

  const handleReject = async (id: string) => {
    if (!confirm("Are you sure you want to reject this policy?")) return;
    await apiClient.post(`/admin/policy-review/${id}/reject`);
    setPolicies((prev) => prev.filter((p) => p.id !== id));
  };

  const toggleExpand = async (policy: Policy) => {
    if (expandedId === policy.id) {
      setExpandedId(null);
      return;
    }
    setExpandedId(policy.id);

    if (!brokers[policy.brokerId]) {
      const brokerData = await getBrokerById(policy.brokerId);
      setBrokers((prev) => ({ ...prev, [policy.brokerId]: brokerData }));
    }
  };

  if (loading) {
    return (
      <div className="space-y-4">
        <div className="h-8 w-64 bg-slate-200 rounded animate-pulse" />
        <div className="h-32 bg-slate-200 rounded-xl animate-pulse" />
        <div className="h-32 bg-slate-200 rounded-xl animate-pulse" />
      </div>
    );
  }

  const DetailItem = ({
    icon,
    label,
    value,
  }: {
    icon?: React.ReactNode;
    label: string;
    value: React.ReactNode;
  }) => (
    <div className="flex items-start justify-between gap-3 py-2 border-b border-slate-100 last:border-0 text-sm">
      <span className="flex items-center gap-1.5 text-slate-500 shrink-0">
        {icon && <span className="text-slate-400">{icon}</span>}
        {label}
      </span>
      <span className="text-slate-900 font-medium text-right">{value}</span>
    </div>
  );

  return (
    <div className="space-y-6">

      {/* HEADER */}
      <div>
        <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
          Policy Review
        </h1>
        <p className="text-sm text-slate-500 mt-0.5">
          {policies.length} polic{policies.length === 1 ? "y" : "ies"} pending approval
        </p>
      </div>

      {/* EMPTY STATE */}
      {policies.length === 0 && (
        <div className="bg-white rounded-xl border border-slate-200/80 p-12 text-center">
          <div className="inline-flex w-12 h-12 items-center justify-center rounded-full bg-emerald-50 text-emerald-600 mb-3">
            <ShieldCheck size={22} />
          </div>
          <p className="text-sm font-medium text-slate-900">All caught up</p>
          <p className="text-sm text-slate-500 mt-1">
            There are no policies waiting for review.
          </p>
        </div>
      )}

      {/* CARDS */}
      <div className="space-y-3">
        {policies.map((policy) => {
          const isExpanded = expandedId === policy.id;
          const broker = brokers[policy.brokerId];

          return (
            <div
              key={policy.id}
              className="bg-white rounded-xl border border-slate-200/80 overflow-hidden"
            >
              {/* SUMMARY ROW */}
              <div className="p-5">
                <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">

                  {/* LEFT: POLICY INFO */}
                  <div className="flex items-start gap-4 min-w-0 flex-1">
                    <div className="w-10 h-10 rounded-lg bg-amber-50 text-amber-600 flex items-center justify-center shrink-0">
                      <ShieldCheck size={18} />
                    </div>
                    <div className="min-w-0 flex-1">
                      <div className="flex items-center gap-2 flex-wrap">
                        <p className="text-sm font-semibold text-slate-900 truncate">
                          {policy.client.name}
                        </p>
                        <span className="inline-flex items-center gap-1.5 px-2 py-0.5 rounded-full text-[11px] font-medium bg-amber-50 text-amber-700">
                          <span className="w-1.5 h-1.5 rounded-full bg-amber-500" />
                          Under Review
                        </span>
                      </div>
                      <p className="text-xs text-slate-500 mt-0.5 font-mono">
                        {policy.policyNumber}
                      </p>

                      <div className="flex items-center gap-4 mt-2 text-xs text-slate-500">
                        <span className="inline-flex items-center gap-1">
                          <MapPin size={12} />
                          {policy.building.geography.city}, {policy.building.geography.county}
                        </span>
                        <span className="inline-flex items-center gap-1">
                          <DollarSign size={12} />
                          <span className="font-medium text-slate-700">
                            {policy.finalPremium.toLocaleString()} {policy.currency.code}
                          </span>
                        </span>
                      </div>
                    </div>
                  </div>

                  {/* RIGHT: ACTIONS */}
                  <div className="flex items-center gap-2 shrink-0">
                    <button
                      onClick={() => toggleExpand(policy)}
                      className="inline-flex items-center gap-1 px-3 py-1.5 rounded-lg text-xs font-medium text-slate-600 border border-slate-200 hover:bg-slate-50 transition-colors"
                    >
                      {isExpanded ? <ChevronUp size={13} /> : <ChevronDown size={13} />}
                      {isExpanded ? "Hide" : "Details"}
                    </button>

                    <button
                      onClick={() => handleApprove(policy.id)}
                      className="inline-flex items-center gap-1 px-3 py-1.5 rounded-lg text-xs font-medium bg-teal-600 text-white hover:bg-teal-700 transition-colors"
                    >
                      <Check size={13} />
                      Approve
                    </button>

                    <button
                      onClick={() => handleReject(policy.id)}
                      className="inline-flex items-center gap-1 px-3 py-1.5 rounded-lg text-xs font-medium text-red-600 border border-red-200 hover:bg-red-50 transition-colors"
                    >
                      <X size={13} />
                      Reject
                    </button>
                  </div>
                </div>
              </div>

              {/* EXPANDED DETAILS */}
              {isExpanded && (
                <div className="border-t border-slate-100 bg-slate-50/50 p-5">
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-4">

                    {/* CLIENT */}
                    <div>
                      <div className="flex items-center gap-2 mb-2">
                        <User size={14} className="text-teal-600" />
                        <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                          Client
                        </p>
                      </div>
                      <DetailItem label="Name" value={policy.client.name} />
                      <DetailItem label="Identification" value={policy.client.identificationNumber} />
                    </div>

                    {/* BROKER */}
                    <div>
                      <div className="flex items-center gap-2 mb-2">
                        <Briefcase size={14} className="text-teal-600" />
                        <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                          Broker
                        </p>
                      </div>
                      {broker ? (
                        <>
                          <DetailItem label="Name" value={broker.name} />
                          <DetailItem label="Email" value={broker.email} />
                          <DetailItem label="Phone" value={broker.phone} />
                        </>
                      ) : (
                        <p className="text-xs text-slate-500">Loading broker...</p>
                      )}
                    </div>

                    {/* BUILDING */}
                    <div>
                      <div className="flex items-center gap-2 mb-2">
                        <Building2 size={14} className="text-teal-600" />
                        <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                          Building
                        </p>
                      </div>
                      <DetailItem
                        label="Address"
                        value={`${policy.building.street} ${policy.building.number}`}
                      />
                      <DetailItem label="City" value={policy.building.geography.city} />
                      <DetailItem label="County" value={policy.building.geography.county} />
                      <DetailItem label="Country" value={policy.building.geography.country} />
                      <DetailItem
                        label="Insured Value"
                        value={policy.building.insuredValue.toLocaleString()}
                      />
                    </div>

                    {/* FINANCIAL + PERIOD */}
                    <div>
                      <div className="flex items-center gap-2 mb-2">
                        <DollarSign size={14} className="text-teal-600" />
                        <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                          Financial
                        </p>
                      </div>
                      <DetailItem
                        label="Base Premium"
                        value={policy.basePremium.toLocaleString()}
                      />
                      <DetailItem
                        label="Final Premium"
                        value={`${policy.finalPremium.toLocaleString()} ${policy.currency.code}`}
                      />
                      <DetailItem
                        label="Currency"
                        value={`${policy.currency.name} (${policy.currency.code})`}
                      />

                      <div className="flex items-center gap-2 mb-2 mt-4">
                        <Calendar size={14} className="text-teal-600" />
                        <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                          Period
                        </p>
                      </div>
                      <DetailItem
                        label="Start"
                        value={new Date(policy.startDate).toLocaleDateString()}
                      />
                      <DetailItem
                        label="End"
                        value={new Date(policy.endDate).toLocaleDateString()}
                      />
                    </div>

                  </div>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
}

export default AdminReviewPoliciesPage;