import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";
import { getBrokerById } from "../api/brokerApi";

interface Policy {
  id: string;
  brokerId: string;
  policyNumber: string;
  status: string;
  finalPremium: number;
  basePremium: number;
  startDate: string;
  endDate: string;

  client: {
    name: string;
    identificationNumber: string;
  };

  building: {
    street: string;
    number: string;
    insuredValue: number;
    geography: {
      city: string;
      county: string;
      country: string;
    };
  };

  currency: {
    code: string;
    name: string;
  };
}

interface PolicyResponse {
  items: Policy[];
}

function AdminReviewPoliciesPage() {
  const [policies, setPolicies] = useState<Policy[]>([]);
  const [loading, setLoading] = useState(true);
  const [expandedId, setExpandedId] = useState<string | null>(null);
  const [brokers, setBrokers] = useState<Record<string, any>>({});

  const fetchPolicies = async () => {
    try {
      const res = await apiClient.get<PolicyResponse>(
        "/admin/policy-review"
      );
      setPolicies(res.data.items);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPolicies();
  }, []);

  const handleApprove = async (id: string) => {
    await apiClient.post(`/admin/policy-review/${id}/approve`);
    setPolicies(prev => prev.filter(p => p.id !== id));
  };

  const handleReject = async (id: string) => {
    if (!confirm("Are you sure you want to reject this policy?")) return;

    await apiClient.post(`/admin/policy-review/${id}/reject`);
    setPolicies(prev => prev.filter(p => p.id !== id));
  };

  const toggleExpand = async (policy : Policy) => {
    if (expandedId === policy.id) {
      setExpandedId(null);
      return;
    }
    setExpandedId(policy.id);

    if(!brokers[policy.brokerId]) {
      const brokerData = await getBrokerById(policy.brokerId);
      setBrokers(prev => ({ ...prev, [policy.brokerId]: brokerData }));
    }
  };


  if (loading) return <div className="p-6">Loading...</div>;

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-2xl font-bold">Policies Under Review</h1>

      {policies.length === 0 && (
        <div className="text-gray-500">No policies to review</div>
      )}

      {policies.map(policy => (
        <div
          key={policy.id}
          className="bg-white p-5 rounded-2xl shadow border hover:shadow-lg transition"
        >
          {/* HEADER */}
          <div className="flex justify-between items-start">
            <div className="space-y-1">
              <div className="font-semibold text-lg">
                {policy.client.name}
              </div>

              <div className="text-sm text-gray-500">
                {policy.building.geography.city},{" "}
                {policy.building.geography.county}
              </div>

              <div className="text-sm">
                {policy.building.street} {policy.building.number}
              </div>

              <div className="text-sm font-medium">
                {policy.finalPremium} {policy.currency.code}
              </div>
            </div>

            {/* STATUS BADGE */}
            <span className="bg-yellow-100 text-yellow-700 px-3 py-1 rounded-full text-xs font-semibold">
              Under Review
            </span>
          </div>

          {/* ACTIONS */}
          <div className="flex gap-2 mt-4">
            <button
              onClick={() => toggleExpand(policy)}
              className="px-3 py-1 text-sm border rounded-lg hover:bg-gray-100"
            >
              {expandedId === policy.id ? "Hide Details" : "View Details"}
            </button>

            <button
              onClick={() => handleApprove(policy.id)}
              className="px-3 py-1 text-sm bg-green-400 text-white rounded-lg hover:bg-green-700 transition"
            >
              Accept
            </button>

            <button
              onClick={() => handleReject(policy.id)}
              className="px-3 py-1 text-sm bg-red-400 text-white rounded-lg hover:bg-red-700 transition"
            >
              Reject
            </button>
          </div>

          {/* DROPDOWN DETAILS */}
          {expandedId === policy.id && (
            <div className="mt-4 p-4 bg-gray-50 rounded-xl text-sm space-y-2 border">
              
              <div><strong>Policy Number:</strong> {policy.policyNumber}</div>
              <div><strong>Status:</strong> {policy.status}</div>

              <div className="pt-2 border-t">
                <strong>Client</strong>
                <div>Name: {policy.client.name}</div>
                <div>ID: {policy.client.identificationNumber}</div>
              </div>

              <div className="pt-2 border-t">
                <strong>Broker</strong>

                {brokers[policy.brokerId] ? (
                  <>
                    <div>Name: {brokers[policy.brokerId].name}</div>
                    <div>Email: {brokers[policy.brokerId].email}</div>
                    <div>Phone: {brokers[policy.brokerId].phone}</div>
                  </>
                ) : (
                  <div>Loading...</div>
                )}
              </div>

              <div className="pt-2 border-t">
                <strong>Building</strong>
                <div>Address: {policy.building.street} {policy.building.number}</div>
                <div>City: {policy.building.geography.city}</div>
                <div>County: {policy.building.geography.county}</div>
                <div>Country: {policy.building.geography.country}</div>
                <div>Insured Value: {policy.building.insuredValue}</div>
              </div>

              <div className="pt-2 border-t">
                <strong>Financial</strong>
                <div>Base Premium: {policy.basePremium}</div>
                <div>Final Premium: {policy.finalPremium}</div>
                <div>Currency: {policy.currency.name} ({policy.currency.code})</div>
              </div>

              <div className="pt-2 border-t">
                <strong>Period</strong>
                <div>Start: {new Date(policy.startDate).toLocaleDateString()}</div>
                <div>End: {new Date(policy.endDate).toLocaleDateString()}</div>
              </div>

            </div>
          )}
        </div>
      ))}
    </div>
  );
}

export default AdminReviewPoliciesPage;