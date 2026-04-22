import { useEffect, useState } from "react";
import { Plus, ChevronLeft, ChevronRight } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { getAllBrokers } from "../api/brokerApi";
import type { Broker } from "../api/brokerApi";
import type { Column } from "../components/DataTable";
import DataTable from "../components/DataTable";
import BrokerRowActions from "../components/BrokerRowActions";

function BrokersPage() {
  const [brokers, setBrokers] = useState<Broker[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const [page, setPage] = useState(1);
  const pageSize = 10;
  const [totalCount, setTotalCount] = useState(0);

  const fetchBrokers = async () => {
    setLoading(true);
    try {
      const response = await getAllBrokers(page, pageSize);
      setBrokers(response.items);
      setTotalCount(response.totalCount);
    } catch (err) {
      console.error("Error fetching brokers", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBrokers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page]);

  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));

  const columns: Column<Broker>[] = [
    {
      header: "Name",
      render: (broker) => (
        <span className="font-medium text-slate-900">{broker.name}</span>
      ),
    },
    {
      header: "Broker Code",
      render: (broker) => (
        <span className="inline-flex items-center px-2 py-0.5 rounded-md text-xs font-medium bg-slate-100 text-slate-600 font-mono">
          {broker.brokerCode}
        </span>
      ),
    },
    { header: "Email", accessor: "email" },
    { header: "Phone", accessor: "phone" },
    {
      header: "Status",
      render: (broker) => (
        <span
          className={`inline-flex items-center gap-1.5 px-2 py-0.5 rounded-full text-xs font-medium ${
            broker.isActive
              ? "bg-emerald-50 text-emerald-700"
              : "bg-slate-100 text-slate-500"
          }`}
        >
          <span
            className={`w-1.5 h-1.5 rounded-full ${
              broker.isActive ? "bg-emerald-500" : "bg-slate-400"
            }`}
          />
          {broker.isActive ? "Active" : "Inactive"}
        </span>
      ),
    },
    {
      header: "",
      render: (broker) => (
        <BrokerRowActions
          brokerId={broker.id}
          isActive={broker.isActive}
          onStatusChange={fetchBrokers}
        />
      ),
    },
  ];

  const getPageNumbers = () => {
    const pages = [];
    for (let i = 1; i <= totalPages; i++) {
      pages.push(i);
    }
    return pages;
  };

  if (loading) {
    return (
      <div className="space-y-4">
        <div className="h-8 w-48 bg-slate-200 rounded animate-pulse" />
        <div className="h-64 bg-slate-200 rounded-xl animate-pulse" />
      </div>
    );
  }

  return (
    <div className="space-y-6">

      {/* HEADER */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-xl font-semibold text-slate-900 tracking-tight">
            Brokers
          </h1>
          <p className="text-sm text-slate-500 mt-0.5">
            {totalCount} total broker{totalCount !== 1 ? "s" : ""} on the platform
          </p>
        </div>

        <button
          onClick={() => navigate("/admin/brokers/create")}
          className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          <Plus size={15} />
          Add Broker
        </button>
      </div>

      {/* TABLE */}
      <DataTable<Broker> data={brokers} columns={columns} />

      {/* PAGINATION */}
      <div className="flex items-center justify-between">
        <span className="text-sm text-slate-500">
          Page <span className="font-medium text-slate-700">{page}</span> of{" "}
          <span className="font-medium text-slate-700">{totalPages}</span>
        </span>

        <div className="flex gap-1 items-center">
          <button
            disabled={page === 1}
            onClick={() => setPage((p) => p - 1)}
            className="p-1.5 rounded-md border border-slate-200 bg-white hover:bg-slate-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
          >
            <ChevronLeft size={16} />
          </button>

          {getPageNumbers().map((p) => (
            <button
              key={p}
              onClick={() => setPage(p)}
              className={`
                w-8 h-8 rounded-md text-sm font-medium transition-colors
                ${p === page
                  ? "bg-teal-600 text-white"
                  : "text-slate-600 hover:bg-slate-100"}
              `}
            >
              {p}
            </button>
          ))}

          <button
            disabled={page === totalPages}
            onClick={() => setPage((p) => p + 1)}
            className="p-1.5 rounded-md border border-slate-200 bg-white hover:bg-slate-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
          >
            <ChevronRight size={16} />
          </button>
        </div>
      </div>

    </div>
  );
}

export default BrokersPage;