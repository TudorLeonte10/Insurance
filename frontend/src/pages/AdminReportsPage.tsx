import { useCallback, useEffect, useMemo, useState } from "react";
import {
  LineChart, Line, BarChart, Bar, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
} from "recharts";
import {
  getPolicySummary,
  getPoliciesReport,
  getPolicyTimeseries,
  type PolicySummaryDto,
  type PolicyTimeseriesDto,
  type PolicyReportDto,
  type ReportFilters,
} from "../api/policyReportsApi";
import { ShieldCheck, FileText, TrendingUp, Clock } from "lucide-react";

const STATUS_OPTIONS = ["Active", "Draft", "UnderReview", "Cancelled", "Expired", "Rejected"];
const BUILDING_TYPES = ["Residential", "Commercial", "Industrial"];
const CHART_COLORS = ["#6366f1", "#22c55e", "#f59e0b", "#ef4444", "#3b82f6", "#ec4899", "#14b8a6"];
const PIE_COLORS = ["#6366f1", "#22c55e", "#f59e0b", "#ef4444", "#3b82f6", "#ec4899"];

function formatCompact(n: number) {
  return new Intl.NumberFormat("en-US", { notation: "compact", maximumFractionDigits: 1 }).format(n);
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "2-digit" });
}

interface KpiCardProps {
  title: string;
  value: string | number;
  icon: React.ReactNode;
  color: string;
  loading: boolean;
}

function KpiCard({ title, value, icon, color, loading }: KpiCardProps) {
  return (
    <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-5 flex items-center gap-4">
      <div className={`rounded-xl p-3 ${color}`}>{icon}</div>
      <div>
        <p className="text-xs font-medium text-gray-500 uppercase tracking-wide">{title}</p>
        {loading ? (
          <div className="h-7 w-24 bg-gray-100 rounded animate-pulse mt-1" />
        ) : (
          <p className="text-2xl font-bold text-gray-800">{value}</p>
        )}
      </div>
    </div>
  );
}

function ChartSkeleton() {
  return <div className="w-full h-64 bg-gray-100 rounded-xl animate-pulse" />;
}

function aggregateByGroup(data: PolicyReportDto[]): { name: string; count: number; premium: number }[] {
  const map = new Map<string, { count: number; premium: number }>();
  for (const row of data) {
    const existing = map.get(row.groupName) ?? { count: 0, premium: 0 };
    map.set(row.groupName, {
      count: existing.count + row.policiesCount,
      premium: existing.premium + row.totalPremiumInBase,
    });
  }
  return Array.from(map.entries())
    .map(([name, v]) => ({ name, count: v.count, premium: v.premium }))
    .sort((a, b) => b.count - a.count);
}

export default function AdminReportsPage() {
  const [filters, setFilters] = useState<ReportFilters>({});
  const [applied, setApplied] = useState<ReportFilters>({});

  const [summary, setSummary] = useState<PolicySummaryDto | null>(null);
  const [timeseries, setTimeseries] = useState<PolicyTimeseriesDto[]>([]);
  const [cityData, setCityData] = useState<PolicyReportDto[]>([]);
  const [brokerData, setBrokerData] = useState<PolicyReportDto[]>([]);
  const [statusData, setStatusData] = useState<PolicyReportDto[]>([]);
  const [buildingTypeData, setBuildingTypeData] = useState<PolicyReportDto[]>([]);

  const [loadingSummary, setLoadingSummary] = useState(true);
  const [loadingTimeseries, setLoadingTimeseries] = useState(true);
  const [loadingCity, setLoadingCity] = useState(true);
  const [loadingBroker, setLoadingBroker] = useState(true);
  const [loadingStatus, setLoadingStatus] = useState(true);
  const [loadingBuilding, setLoadingBuilding] = useState(true);

  const fetchAll = useCallback(async (f: ReportFilters) => {
    setLoadingSummary(true);
    setLoadingTimeseries(true);
    setLoadingCity(true);
    setLoadingBroker(true);
    setLoadingStatus(true);
    setLoadingBuilding(true);

    getPolicySummary(f).then(setSummary).catch(console.error).finally(() => setLoadingSummary(false));
    getPolicyTimeseries(f).then(setTimeseries).catch(console.error).finally(() => setLoadingTimeseries(false));
    getPoliciesReport("City", f).then(setCityData).catch(console.error).finally(() => setLoadingCity(false));
    getPoliciesReport("Broker", f).then(setBrokerData).catch(console.error).finally(() => setLoadingBroker(false));
    getPoliciesReport("Status", f).then(setStatusData).catch(console.error).finally(() => setLoadingStatus(false));
    getPoliciesReport("BuildingType", f).then(setBuildingTypeData).catch(console.error).finally(() => setLoadingBuilding(false));
  }, []);

  useEffect(() => {
    fetchAll(applied);
  }, [applied, fetchAll]);

  const cityAgg = useMemo(() => aggregateByGroup(cityData).slice(0, 10), [cityData]);
  const brokerAgg = useMemo(() => aggregateByGroup(brokerData).slice(0, 10), [brokerData]);
  const statusAgg = useMemo(() => aggregateByGroup(statusData), [statusData]);
  const buildingAgg = useMemo(() => aggregateByGroup(buildingTypeData), [buildingTypeData]);

  const tsData = useMemo(
    () => timeseries.map((d) => ({ ...d, dateLabel: formatDate(d.date) })),
    [timeseries]
  );

  function handleApply() {
    setApplied({ ...filters });
  }

  function handleReset() {
    setFilters({});
    setApplied({});
  }

  return (
    <div className="p-6 space-y-8 bg-gray-50 min-h-screen">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-gray-900">Policy Analytics</h1>
        <p className="text-gray-500 mt-1">Overview of all insurance policies across the platform.</p>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-5">
        <h2 className="text-sm font-semibold text-gray-700 mb-4 uppercase tracking-wide">Filters</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500 font-medium">From</label>
            <input
              type="date"
              value={filters.from?.substring(0, 10) ?? ""}
              onChange={(e) =>
                setFilters((f) => ({ ...f, from: e.target.value ? e.target.value + "T00:00:00Z" : undefined }))
              }
              className="border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500 font-medium">To</label>
            <input
              type="date"
              value={filters.to?.substring(0, 10) ?? ""}
              onChange={(e) =>
                setFilters((f) => ({ ...f, to: e.target.value ? e.target.value + "T23:59:59Z" : undefined }))
              }
              className="border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500 font-medium">Status</label>
            <select
              value={filters.status ?? ""}
              onChange={(e) => setFilters((f) => ({ ...f, status: e.target.value || undefined }))}
              className="border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
            >
              <option value="">All</option>
              {STATUS_OPTIONS.map((s) => (
                <option key={s} value={s}>{s}</option>
              ))}
            </select>
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500 font-medium">Currency</label>
            <input
              type="text"
              placeholder="RON / EUR / GBP"
              maxLength={3}
              value={filters.currency ?? ""}
              onChange={(e) => setFilters((f) => ({ ...f, currency: e.target.value || undefined }))}
              className="border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500 font-medium">Building Type</label>
            <select
              value={filters.buildingType ?? ""}
              onChange={(e) => setFilters((f) => ({ ...f, buildingType: e.target.value || undefined }))}
              className="border rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
            >
              <option value="">All</option>
              {BUILDING_TYPES.map((t) => (
                <option key={t} value={t}>{t}</option>
              ))}
            </select>
          </div>
        </div>
        <div className="flex gap-3 mt-4">
          <button
            onClick={handleApply}
            className="px-5 py-2 bg-indigo-600 text-white text-sm font-semibold rounded-lg hover:bg-indigo-700 transition"
          >
            Apply Filters
          </button>
          <button
            onClick={handleReset}
            className="px-5 py-2 border text-sm font-semibold rounded-lg hover:bg-gray-50 transition"
          >
            Reset
          </button>
        </div>
      </div>

      {/* KPI Cards */}
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
        <KpiCard
          title="Total Policies"
          value={summary?.totalPolicies ?? 0}
          icon={<FileText size={20} className="text-indigo-600" />}
          color="bg-indigo-50"
          loading={loadingSummary}
        />
        <KpiCard
          title="Total Premium"
          value={summary ? formatCompact(summary.totalPremium) : "—"}
          icon={<TrendingUp size={20} className="text-green-600" />}
          color="bg-green-50"
          loading={loadingSummary}
        />
        <KpiCard
          title="Avg Premium"
          value={summary ? formatCompact(summary.averagePremium) : "—"}
          icon={<ShieldCheck size={20} className="text-blue-600" />}
          color="bg-blue-50"
          loading={loadingSummary}
        />
        <KpiCard
          title="Active"
          value={summary?.activePolicies ?? 0}
          icon={<ShieldCheck size={20} className="text-emerald-600" />}
          color="bg-emerald-50"
          loading={loadingSummary}
        />
        <KpiCard
          title="Under Review"
          value={summary?.underReviewPolicies ?? 0}
          icon={<Clock size={20} className="text-amber-600" />}
          color="bg-amber-50"
          loading={loadingSummary}
        />
      </div>

      {/* Timeseries Line Chart */}
      <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-6">
        <h2 className="text-base font-semibold text-gray-800 mb-4">Policies Over Time</h2>
        {loadingTimeseries ? (
          <ChartSkeleton />
        ) : tsData.length === 0 ? (
          <div className="flex items-center justify-center h-64 text-gray-400">No data available</div>
        ) : (
          <ResponsiveContainer width="100%" height={280}>
            <LineChart data={tsData} margin={{ top: 5, right: 20, left: 0, bottom: 5 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
              <XAxis dataKey="dateLabel" tick={{ fontSize: 11 }} />
              <YAxis tick={{ fontSize: 11 }} />
              <Tooltip />
              <Legend />
              <Line
                type="monotone"
                dataKey="policyCount"
                name="Policy Count"
                stroke="#6366f1"
                strokeWidth={2}
                dot={false}
              />
            </LineChart>
          </ResponsiveContainer>
        )}
      </div>

      {/* Bar Charts Row */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Policies by Country */}
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-6">
          <h2 className="text-base font-semibold text-gray-800 mb-4">Policies by City</h2>
          {loadingCity ? (
            <ChartSkeleton />
          ) : cityAgg.length === 0 ? (
            <div className="flex items-center justify-center h-64 text-gray-400">No data available</div>
          ) : (
            <ResponsiveContainer width="100%" height={260}>
              <BarChart data={cityAgg} layout="vertical" margin={{ left: 10, right: 20 }}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" horizontal={false} />
                <XAxis type="number" tick={{ fontSize: 11 }} />
                <YAxis type="category" dataKey="name" tick={{ fontSize: 11 }} width={80} />
                <Tooltip formatter={(v) => v} />
                <Bar dataKey="count" name="Policies" radius={[0, 4, 4, 0]}>
                  {cityAgg.map((_, i) => (
                    <Cell key={i} fill={CHART_COLORS[i % CHART_COLORS.length]} />
                  ))}
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>

        {/* Top Brokers */}
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-6">
          <h2 className="text-base font-semibold text-gray-800 mb-4">Top Brokers by Policies</h2>
          {loadingBroker ? (
            <ChartSkeleton />
          ) : brokerAgg.length === 0 ? (
            <div className="flex items-center justify-center h-64 text-gray-400">No data available</div>
          ) : (
            <ResponsiveContainer width="100%" height={260}>
              <BarChart data={brokerAgg} layout="vertical" margin={{ left: 10, right: 20 }}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" horizontal={false} />
                <XAxis type="number" tick={{ fontSize: 11 }} />
                <YAxis type="category" dataKey="name" tick={{ fontSize: 11 }} width={80} />
                <Tooltip formatter={(v) => v} />
                <Bar dataKey="count" name="Policies" radius={[0, 4, 4, 0]}>
                  {brokerAgg.map((_, i) => (
                    <Cell key={i} fill={CHART_COLORS[i % CHART_COLORS.length]} />
                  ))}
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      {/* Pie Charts Row */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Status Distribution */}
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-6">
          <h2 className="text-base font-semibold text-gray-800 mb-4">Policy Status Distribution</h2>
          {loadingStatus ? (
            <ChartSkeleton />
          ) : statusAgg.length === 0 ? (
            <div className="flex items-center justify-center h-64 text-gray-400">No data available</div>
          ) : (
            <div className="flex items-center gap-4">
              <ResponsiveContainer width="55%" height={240}>
                <PieChart>
                  <Pie
                    data={statusAgg}
                    dataKey="count"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    innerRadius={55}
                    outerRadius={95}
                    paddingAngle={3}
                  >
                    {statusAgg.map((_, i) => (
                      <Cell key={i} fill={PIE_COLORS[i % PIE_COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip formatter={(v) => v} />
                </PieChart>
              </ResponsiveContainer>
              <div className="flex flex-col gap-2 text-sm flex-1">
                {statusAgg.map((entry, i) => (
                  <div key={entry.name} className="flex items-center gap-2">
                    <span
                      className="w-3 h-3 rounded-full shrink-0"
                      style={{ backgroundColor: PIE_COLORS[i % PIE_COLORS.length] }}
                    />
                    <span className="text-gray-600 truncate">{entry.name}</span>
                    <span className="font-semibold ml-auto">{entry.count}</span>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        {/* Building Type Distribution */}
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 p-6">
          <h2 className="text-base font-semibold text-gray-800 mb-4">Building Type Distribution</h2>
          {loadingBuilding ? (
            <ChartSkeleton />
          ) : buildingAgg.length === 0 ? (
            <div className="flex items-center justify-center h-64 text-gray-400">No data available</div>
          ) : (
            <div className="flex items-center gap-4">
              <ResponsiveContainer width="55%" height={240}>
                <PieChart>
                  <Pie
                    data={buildingAgg}
                    dataKey="count"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    innerRadius={55}
                    outerRadius={95}
                    paddingAngle={3}
                  >
                    {buildingAgg.map((_, i) => (
                      <Cell key={i} fill={PIE_COLORS[i % PIE_COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip formatter={(v) => v} />
                </PieChart>
              </ResponsiveContainer>
              <div className="flex flex-col gap-2 text-sm flex-1">
                {buildingAgg.map((entry, i) => (
                  <div key={entry.name} className="flex items-center gap-2">
                    <span
                      className="w-3 h-3 rounded-full shrink-0"
                      style={{ backgroundColor: PIE_COLORS[i % PIE_COLORS.length] }}
                    />
                    <span className="text-gray-600 truncate">{entry.name}</span>
                    <span className="font-semibold ml-auto">{entry.count}</span>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
