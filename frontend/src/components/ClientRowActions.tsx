import { Pencil, Building } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface Props {
  clientId: string | number;
}

function ClientRowActions({ clientId }: Props) {
  const navigate = useNavigate();

  return (
    <div className="flex gap-1">

      {/* Add Building */}
      <button
        onClick={() => navigate(`/broker/clients/${clientId}/buildings/create`)}
        className="p-1.5 rounded-md text-slate-400 hover:text-teal-600 hover:bg-teal-50 transition-colors"
        title="Add building"
      >
        <Building size={16} />
      </button>

      {/* Edit Client */}
      <button
        onClick={() => navigate(`/broker/clients/${clientId}/edit`)}
        className="p-1.5 rounded-md text-slate-400 hover:text-slate-700 hover:bg-slate-100 transition-colors"
        title="Edit client"
      >
        <Pencil size={16} />
      </button>

    </div>
  );
}

export default ClientRowActions;