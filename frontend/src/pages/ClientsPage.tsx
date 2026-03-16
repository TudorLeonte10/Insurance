import { useEffect, useState, useMemo } from "react";
import { Search } from "lucide-react";
import { getBrokerClients } from "../api/brokerApi";
import type { Client } from "../api/brokerApi";
import type { Column } from "../components/DataTable";
import DataTable from "../components/DataTable";
import ClientRowActions from "../components/ClientRowActions";

function ClientsPage() {

  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");

  useEffect(() => {
    const fetchClients = async () => {
      try {
        const data = await getBrokerClients();
        setClients(data);
      } catch (err) {
        console.error("Error fetching clients", err);
      } finally {
        setLoading(false);
      }
    };

    fetchClients();
  }, []);

  const filteredClients = useMemo(() => {
    return clients.filter((c) =>
      c.name.toLowerCase().includes(search.toLowerCase()) ||
      c.identificationNumber.toLowerCase().includes(search.toLowerCase())
    );
  }, [clients, search]);

  const columns: Column<Client>[] = [
    { header: "Name", accessor: "name" },
    { header: "Identification Number", accessor: "identificationNumber" },
    { header: "Type", accessor: "type" },
    { header: "Email", accessor: "email" },
    { header: "Phone", accessor: "phoneNumber" },
    { header: "Address", accessor: "address" },

    {
      header: "Actions",
      render: (client) => (
        <ClientRowActions clientId={client.id} />
      ),
    },
  ];

  if (loading) {
    return (
      <div className="p-6 text-gray-500">
        Loading clients...
      </div>
    );
  }

  return (
    <div className="space-y-6 p-6">

      {/* HEADER */}
      <div className="flex justify-between items-center">

        <h1 className="text-2xl font-bold text-gray-800">
          Clients
        </h1>

        <button className="bg-[#00204a] text-white px-4 py-2 rounded-lg shadow hover:opacity-90">
          + Create Client
        </button>

      </div>

      {/* SEARCH */}
      <div className="relative max-w-sm">

        <Search
          size={18}
          className="absolute left-3 top-3 text-gray-400"
        />

        <input
          type="text"
          placeholder="Search clients..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="pl-9 pr-4 py-2 border rounded-lg w-full focus:ring-2 focus:ring-[#00204a]"
        />

      </div>

      {/* TABLE */}
      <DataTable<Client>
        data={filteredClients}
        columns={columns}
      />

    </div>
  );
}

export default ClientsPage;