import { useEffect, useState } from "react";
import { Search, Plus, ChevronLeft, ChevronRight } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { getBrokerClients } from "../api/brokerApi";
import type { Client } from "../api/brokerApi";
import type { Column } from "../components/DataTable";
import DataTable from "../components/DataTable";
import ClientRowActions from "../components/ClientRowActions";

function ClientsPage() {
  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");

  const [page, setPage] = useState(1);
  const pageSize = 10;

  const [totalCount, setTotalCount] = useState(0);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, 400);

    return () => clearTimeout(timer);
  }, [search]);

  useEffect(() => {
    const fetchClients = async () => {
      setLoading(true);

      try {
        const response = await getBrokerClients(page, pageSize, debouncedSearch);

        setClients(response.items);
        setTotalCount(response.totalCount);

      } catch (err) {
        console.error("Error fetching clients", err);
      } finally {
        setLoading(false);
      }
    };

    fetchClients();
  }, [page, debouncedSearch]);

  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));

  const columns: Column<Client>[] = [
    {
      header: "Name",
      render: (client) => (
        <span className="font-medium text-slate-900">{client.name}</span>
      ),
    },
    { header: "ID Number", accessor: "identificationNumber" },
    {
      header: "Type",
      render: (client) => (
        <span className="inline-flex items-center px-2 py-0.5 rounded-md text-xs font-medium bg-slate-100 text-slate-600">
          {client.type}
        </span>
      ),
    },
    { header: "Email", accessor: "email" },
    { header: "Phone", accessor: "phoneNumber" },
    {
      header: "",
      render: (client) => (
        <ClientRowActions clientId={client.id} />
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
        <div className="h-10 w-72 bg-slate-200 rounded animate-pulse" />
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
            Clients
          </h1>
          <p className="text-sm text-slate-500 mt-0.5">
            {totalCount} total client{totalCount !== 1 ? "s" : ""} in your portfolio
          </p>
        </div>

        <button
          onClick={() => navigate("/broker/clients/create")}
          className="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium rounded-lg bg-teal-600 text-white hover:bg-teal-700 transition-colors"
        >
          <Plus size={15} />
          Add Client
        </button>
      </div>

      {/* SEARCH */}
      <div className="relative max-w-xs">
        <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />

        <input
          type="text"
          placeholder="Search by name..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="pl-9 pr-4 py-2 text-sm border border-slate-200 rounded-lg w-full bg-white focus:outline-none focus:ring-2 focus:ring-teal-500/20 focus:border-teal-500"
        />
      </div>

      {/* TABLE */}
      <DataTable<Client>
        data={clients}
        columns={columns}
      />

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

export default ClientsPage;