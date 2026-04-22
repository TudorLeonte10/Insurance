import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getPolicyById, activatePolicy, cancelPolicy } from "../api/policyApi";
import { getBuildingsByClient } from "../api/buildingApi";
import { ArrowLeft, FileText, User, Building2, MapPin, Shield, Calendar, DollarSign, Layers, AlertTriangle, Power, XCircle } from "lucide-react";

function PolicyDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [policy, setPolicy] = useState<any>(null);
  const [buildingDetails, setBuildingDetails] = useState<any>(null);
  const [actionMsg, setActionMsg] = useState({ type: "", text: "" });

  useEffect(() => {
    const fetchData = async () => {
      if (!id) return;

      const policyData = await getPolicyById(id);
      setPolicy(policyData);

      const buildings = await getBuildingsByClient(
        policyData.client.id
      );

      const fullBuilding = buildings.find(
        (b: any) => b.id === policyData.building.id
      );

      setBuildingDetails(fullBuilding);
    };

    fetchData();
  }, [id]);

  const handleActivate = async () => {
    try {
      await activatePolicy(id!);
      setActionMsg({ type: "success", text: "Policy activated successfully." });
      const updated = await getPolicyById(id!);
      setPolicy(updated);
    } catch (err: any) {
      setActionMsg({ type: "error", text: err?.response?.data?.message || err?.response?.data || "Activation failed." });
    }
  };

  const handleCancel = async () => {
    try {
      await cancelPolicy(id!);
      setActionMsg({ type: "success", text: "Policy cancelled." });
      const updated = await getPolicyById(id!);
      setPolicy(updated);
    } catch (err: any) {
      setActionMsg({ type: "error", text: err?.response?.data?.message || err?.response?.data || "Cancellation failed." });
    }
  };

  const statusConfig: Record<string, { label: string; bg: string; text: string; dot: string }> = {
    draft:       { label: "Draft",        bg: "bg-amber-50",   text: "text-amber-700",   dot: "bg-amber-500" },
    underReview: { label: "Under Review", bg: "bg-orange-50",  text: "text-orange-700",  dot: "bg-orange-500" },
    active:      { label: "Active",       bg: "bg-emerald-50", text: "text-emerald-700", dot: "bg-emerald-500" },
    cancelled:   { label: "Cancelled",    bg: "bg-slate-100",  text: "text-slate-500",   dot: "bg-slate-400" },
    expired:     { label: "Expired",      bg: "bg-red-50",     text: "text-red-600",     dot: "bg-red-400" },
  };

  if (!policy) {
    return (
      <div className="space-y-4">
        <div className="h-8 w-48 bg-slate-200 rounded animate-pulse" />
        <div className="h-32 bg-slate-200 rounded-xl animate-pulse" />
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
          <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
          <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
        </div>
      </div>
    );
  }

  const status = statusConfig[policy.status] || statusConfig.draft;

  const startDate = new Date(policy.startDate);
  const endDate = new Date(policy.endDate);
  const totalDays = Math.ceil((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24));
  const elapsed = Math.ceil((Date.now() - startDate.getTime()) / (1000 * 60 * 60 * 24));
  const progress = Math.max(0, Math.min(100, (elapsed / totalDays) * 100));

  const DetailRow = ({ label, value, icon }: { label: string; value: React.ReactNode; icon?: React.ReactNode }) => (
    <div className="flex items-center justify-between py-3 border-b border-slate-100 last:border-0">
      <span className="flex items-center gap-2 text-sm text-slate-500">
        {icon && <span className="text-slate-400">{icon}</span>}
        {label}
      </span>
      <span className="text-sm font-medium text-slate-900 text-right">{value}</span>
    </div>
  );

  return (
    <div className="space-y-6">

      {/* BACK + HEADER */}
      <div>
        <button
          onClick={() => navigate("/broker/policies")}
          className="inline-flex items-center gap-1.5 text-sm text-slate-500 hover:text-teal-600 transition-colors mb-4"
        >
          <ArrowLeft size={15} />
          Back to Policies
        </button>

        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
              {policy.policyNumber}
            </h1>
            <p className="text-sm text-slate-500 mt-0.5">
              {policy.client.name}
            </p>
          </div>

          <div className="flex items-center gap-2">
            <span className={`inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-xs font-medium ${status.bg} ${status.text}`}>
              <span className={`w-1.5 h-1.5 rounded-full ${status.dot}`} />
              {status.label}
            </span>

            {policy.status === "draft" && (
              <button
                onClick={handleActivate}
                className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium bg-teal-600 text-white hover:bg-teal-700 transition-colors"
              >
                <Power size={13} />
                Activate
              </button>
            )}

            {policy.status !== "cancelled" && policy.status !== "underReview" && policy.status !== "expired" && (
              <button
                onClick={handleCancel}
                className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium text-red-600 border border-red-200 hover:bg-red-50 transition-colors"
              >
                <XCircle size={13} />
                Cancel
              </button>
            )}
          </div>
        </div>
      </div>

      {/* ACTION MESSAGES */}
      {actionMsg.text && (
        <div className={`flex items-center gap-2 text-sm px-4 py-3 rounded-lg ${
          actionMsg.type === "success"
            ? "bg-emerald-50 border border-emerald-200 text-emerald-700"
            : "bg-red-50 border border-red-200 text-red-700"
        }`}>
          {actionMsg.text}
        </div>
      )}

      {/* PREMIUM HERO CARD */}
      <div className="bg-white rounded-xl border border-slate-200/80 p-6">
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-6">
          <div>
            <p className="text-xs text-slate-500 mb-1">Base Premium</p>
            <p className="text-lg font-semibold text-slate-900">
              {policy.basePremium?.toLocaleString()}
            </p>
          </div>
          <div>
            <p className="text-xs text-slate-500 mb-1">Final Premium</p>
            <p className="text-lg font-semibold text-teal-700">
              {policy.finalPremium?.toLocaleString()} <span className="text-sm font-normal text-slate-500">{policy.currency.code}</span>
            </p>
          </div>
          <div>
            <p className="text-xs text-slate-500 mb-1">Duration</p>
            <p className="text-lg font-semibold text-slate-900">
              {totalDays} <span className="text-sm font-normal text-slate-500">days</span>
            </p>
          </div>
          <div>
            <p className="text-xs text-slate-500 mb-1">Insured Value</p>
            <p className="text-lg font-semibold text-slate-900">
              {policy.building.insuredValue?.toLocaleString()}
            </p>
          </div>
        </div>

        {/* TIMELINE PROGRESS */}
        {policy.status === "active" && (
          <div className="mt-6 pt-5 border-t border-slate-100">
            <div className="flex justify-between text-xs text-slate-500 mb-2">
              <span>{startDate.toLocaleDateString()}</span>
              <span>{Math.round(progress)}% elapsed</span>
              <span>{endDate.toLocaleDateString()}</span>
            </div>
            <div className="h-2 bg-slate-100 rounded-full overflow-hidden">
              <div
                className="h-full bg-teal-500 rounded-full transition-all duration-500"
                style={{ width: `${progress}%` }}
              />
            </div>
          </div>
        )}
      </div>

      {/* TWO COLUMN LAYOUT */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">

        {/* POLICY DETAILS */}
        <div className="bg-white rounded-xl border border-slate-200/80 p-6">
          <div className="flex items-center gap-2 mb-4">
            <FileText size={15} className="text-teal-600" />
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Policy Details</p>
          </div>
          <DetailRow label="Policy Number" value={policy.policyNumber} />
          <DetailRow label="Start Date" value={startDate.toLocaleDateString()} icon={<Calendar size={13} />} />
          <DetailRow label="End Date" value={endDate.toLocaleDateString()} icon={<Calendar size={13} />} />
          <DetailRow label="Base Premium" value={policy.basePremium?.toLocaleString()} icon={<DollarSign size={13} />} />
          <DetailRow label="Final Premium" value={`${policy.finalPremium?.toLocaleString()} ${policy.currency.code}`} icon={<DollarSign size={13} />} />
        </div>

        {/* CLIENT DETAILS */}
        <div className="bg-white rounded-xl border border-slate-200/80 p-6">
          <div className="flex items-center gap-2 mb-4">
            <User size={15} className="text-teal-600" />
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Client</p>
          </div>
          <DetailRow label="Name" value={policy.client.name} />
          <DetailRow label="Identification" value={policy.client.identificationNumber} />
        </div>
      </div>

      {/* BUILDING CARD */}
      <div className="bg-white rounded-xl border border-slate-200/80 p-6">
        <div className="flex items-center gap-2 mb-4">
          <Building2 size={15} className="text-teal-600" />
          <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Building</p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-x-8">
          <div>
            <DetailRow label="Address" value={`${policy.building.street} ${policy.building.number}`} icon={<MapPin size={13} />} />
            <DetailRow label="City" value={policy.building.geography.city} />
            <DetailRow label="County" value={policy.building.geography.county} />
            <DetailRow label="Country" value={policy.building.geography.country} />
            <DetailRow label="Insured Value" value={policy.building.insuredValue?.toLocaleString()} icon={<Shield size={13} />} />
          </div>

          {buildingDetails && (
            <div>
              <DetailRow label="Type" value={buildingDetails.type} icon={<Building2 size={13} />} />
              <DetailRow label="Construction Year" value={buildingDetails.constructionYear} icon={<Calendar size={13} />} />
              <DetailRow label="Surface Area" value={`${buildingDetails.surfaceArea} m\u00B2`} icon={<Layers size={13} />} />
              <DetailRow label="Floors" value={buildingDetails.numberOfFloors} icon={<Layers size={13} />} />
            </div>
          )}
        </div>
      </div>

      {/* RISK INDICATORS */}
      {buildingDetails && buildingDetails.riskIndicators.length > 0 && (
        <div className="bg-white rounded-xl border border-slate-200/80 p-6">
          <div className="flex items-center gap-2 mb-4">
            <AlertTriangle size={15} className="text-amber-600" />
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Risk Indicators</p>
          </div>
          <div className="flex flex-wrap gap-2">
            {buildingDetails.riskIndicators.map((ri: string, idx: number) => (
              <span
                key={idx}
                className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium bg-amber-50 text-amber-700 border border-amber-200"
              >
                <AlertTriangle size={11} />
                {ri}
              </span>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default PolicyDetailsPage;