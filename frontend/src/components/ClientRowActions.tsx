import { Eye, Pencil, Building } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface Props {
  clientId: string | number;
}

function ClientRowActions({ clientId }: Props) {
  const navigate = useNavigate();

  return (
    <div className="flex gap-3">

      {/* View Client */}
      <button
        onClick={() => navigate(`/broker/clients/${clientId}`)}
        className="text-blue-600 hover:text-blue-800"
        title="View client"
      >
        <Eye size={18} />
      </button>

      {/* Add Building */}
      <button
        onClick={() => navigate(`/broker/clients/${clientId}/buildings/create`)}
        className="text-green-600 hover:text-green-800"
        title="Add building"
      >
        <Building size={18} />
      </button>

      {/* Edit Client */}
      <button
        onClick={() => navigate(`/broker/clients/${clientId}/edit`)}
        className="text-gray-600 hover:text-black"
        title="Edit client"
      >
        <Pencil size={18} />
      </button>

    </div>
  );
}

export default ClientRowActions;