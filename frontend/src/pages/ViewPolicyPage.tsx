import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getPolicyById} from "../api/policyApi";
import { getBuildingsByClient } from "../api/buildingApi";

function PolicyDetailsPage() {
  const { id } = useParams();

  const [policy, setPolicy] = useState<any>(null);
  const [buildingDetails, setBuildingDetails] = useState<any>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!id) return;

      const policyData = await getPolicyById(id);
      setPolicy(policyData);

      const buildings = await getBuildingsByClient(
        policyData.client.id
      );

      const fullBuilding = buildings.find(
        (b: any) => b.id === policyData.building.id
      );

      setBuildingDetails(fullBuilding);
    };

    fetchData();
  }, [id]);

  if (!policy) return <div>Loading...</div>;

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">Policy Details</h1>

      <div className="bg-white p-6 rounded shadow space-y-4">

        {/* POLICY */}
        <div>
          <h2 className="font-semibold text-lg">Policy Info</h2>
          <div><b>Number:</b> {policy.policyNumber}</div>
          <div><b>Status:</b> {policy.status}</div>
          <div><b>Base Premium:</b> {policy.basePremium}</div>
          <div><b>Final Premium:</b> {policy.finalPremium} {policy.currency.code}</div>
          <div><b>Start:</b> {policy.startDate}</div>
          <div><b>End:</b> {policy.endDate}</div>
        </div>

        <hr />

        {/* CLIENT */}
        <div>
          <h2 className="font-semibold text-lg">Client</h2>
          <div><b>Name:</b> {policy.client.name}</div>
          <div><b>ID:</b> {policy.client.identificationNumber}</div>
        </div>

        <hr />

        {/* BUILDING BASIC */}
        <div>
          <h2 className="font-semibold text-lg">Building</h2>
          <div>
            <b>Address:</b> {policy.building.street} {policy.building.number}
          </div>
          <div>
            <b>City:</b> {policy.building.geography.city}
          </div>
          <div>
            <b>County:</b> {policy.building.geography.county}
          </div>
          <div>
            <b>Country:</b> {policy.building.geography.country}
          </div>
          <div>
            <b>Insured Value:</b> {policy.building.insuredValue}
          </div>
        </div>

        {buildingDetails && (
          <>
            <hr />
            <div>
              <h2 className="font-semibold text-lg">Building Details</h2>

              <div><b>Type:</b> {buildingDetails.type}</div>
              <div><b>Construction Year:</b> {buildingDetails.constructionYear}</div>
              <div><b>Surface:</b> {buildingDetails.surfaceArea} m²</div>
              <div><b>Floors:</b> {buildingDetails.numberOfFloors}</div>

              <div>
                <b>Risk Indicators:</b>{" "}
                {buildingDetails.riskIndicators.length > 0
                  ? buildingDetails.riskIndicators.join(", ")
                  : "None"}
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}

export default PolicyDetailsPage;