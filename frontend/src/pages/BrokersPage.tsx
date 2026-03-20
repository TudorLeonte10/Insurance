import { useEffect, useState } from "react";
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
  }, [page]);

  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));

  const columns: Column<Broker>[] = [
    { header: "Name", accessor: "name" },
    { header: "Broker Code", accessor: "brokerCode" },
    { header: "Email", accessor: "email" },
    { header: "Phone", accessor: "phone" },
    {
    header: "Status",
    render: (broker) => (
        <span
        className={`px-2 py-1 rounded-full text-xs font-medium ${
            broker.isActive
            ? "bg-green-100 text-green-700"
            : "bg-red-100 text-red-700"
        }`}
        >
        {broker.isActive ? "Active" : "Inactive"}
        </span>
    ),
    },
    {
      header: "Actions",
      render: (broker) => (
        <BrokerRowActions brokerId={broker.id} isActive={broker.isActive} onStatusChange={() => fetchBrokers()}/>
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
      <div className="p-6 text-gray-500">
        Loading brokers...
      </div>
    );
  }

  return (
    <div className="space-y-6 p-6">

      {/* HEADER */}
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold text-gray-800">
          Brokers
        </h1>

        <button className="bg-[#00204a] text-white px-4 py-2 rounded-lg shadow hover:opacity-90"
        onClick={() => navigate("/admin/brokers/create")}>
          + Create Broker
        </button>
      </div>

      {/* TABLE */}
      <DataTable<Broker>
        data={brokers}
        columns={columns}
      />

      {/* PAGINATION */}
      <div className="flex items-center justify-between mt-6">

        {/* LEFT */}
        <span className="text-sm text-gray-500">
          Showing page <strong>{page}</strong> of <strong>{totalPages}</strong>
        </span>

        {/* RIGHT */}
        <div className="flex gap-1 items-center">

          {/* PREV */}
          <button
            disabled={page === 1}
            onClick={() => setPage((p) => p - 1)}
            className="px-3 py-1 rounded-md border bg-white hover:bg-gray-100 disabled:opacity-50"
          >
            ←
          </button>

          {/* PAGE NUMBERS */}
          {getPageNumbers().map((p) => (
            <button
              key={p}
              onClick={() => setPage(p)}
              className={`
                px-3 py-1 rounded-md border text-sm
                ${p === page
                  ? "bg-[#00204a] text-white"
                  : "bg-white hover:bg-gray-100"}
              `}
            >
              {p}
            </button>
          ))}

          {/* NEXT */}
          <button
            disabled={page === totalPages}
            onClick={() => setPage((p) => p + 1)}
            className="px-3 py-1 rounded-md border bg-white hover:bg-gray-100 disabled:opacity-50"
          >
            →
          </button>

        </div>
      </div>

    </div>
  );
}

export default BrokersPage; 