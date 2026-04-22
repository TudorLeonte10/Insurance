import { Pencil, Power, PowerOff } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { activateBroker, deactivateBroker } from "../api/brokerApi";

interface Props {
  brokerId: string | number;
  isActive: boolean;
  onStatusChange: () => void;
}

function BrokerRowActions({ brokerId, isActive, onStatusChange }: Props) {
  const navigate = useNavigate();

  const handleStatusChange = async () => {
    try {
      if (isActive) {
        await deactivateBroker(brokerId.toString());
      } else {
        await activateBroker(brokerId.toString());
      }
      onStatusChange();
    } catch (error) {
      console.error("Error changing broker status:", error);
    }
  };

  return (
    <div className="flex gap-1">

      {/* Activate / Deactivate */}
      <button
        onClick={handleStatusChange}
        className={`p-1.5 rounded-md transition-colors ${
          isActive
            ? "text-slate-400 hover:text-red-600 hover:bg-red-50"
            : "text-slate-400 hover:text-emerald-600 hover:bg-emerald-50"
        }`}
        title={isActive ? "Deactivate broker" : "Activate broker"}
      >
        {isActive ? <PowerOff size={16} /> : <Power size={16} />}
      </button>

      {/* Edit Broker */}
      <button
        onClick={() => navigate(`/admin/brokers/${brokerId}/edit`)}
        className="p-1.5 rounded-md text-slate-400 hover:text-slate-700 hover:bg-slate-100 transition-colors"
        title="Edit broker"
      >
        <Pencil size={16} />
      </button>

    </div>
  );
}

export default BrokerRowActions;