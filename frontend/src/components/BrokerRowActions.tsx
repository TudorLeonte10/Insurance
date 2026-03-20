import { Pencil, CheckCircle } from "lucide-react";
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
    try{
        if(isActive) {
            await deactivateBroker(brokerId.toString());
        }
        else {
            await activateBroker(brokerId.toString());
        }
        onStatusChange();        
    } catch (error) {
        console.error("Error changing broker status:", error);
    }
  };


  return (
    <div className="flex gap-3">

      {/* Activate/Deactivate Broker */}
      <button
        onClick={handleStatusChange}
        className={`text-${isActive ? "red" : "green"}-600 hover:text-${isActive ? "red" : "green"}-800`}
        title={isActive ? "Deactivate broker" : "Activate broker"}
      >
        <CheckCircle size={18} />
      </button>

      {/* Edit Broker */}
      <button
        onClick={() => navigate(`/admin/brokers/${brokerId}/edit`)}
        className="text-gray-600 hover:text-black"
        title="Edit broker"
      >
        <Pencil size={18} />
      </button>

    </div>
  );
}

export default BrokerRowActions;