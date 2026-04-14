import { useEffect, useState } from "react";
import {
  MapContainer,
  TileLayer,
  CircleMarker,
  Popup,
  Tooltip,
} from "react-leaflet";
import "leaflet/dist/leaflet.css";
import type { LatLngExpression } from "leaflet";
import apiClient from "../api/apiClient";
import { cityCoordinates } from "../utils/cityCoordinates";
import { useAuth } from "../auth/AuthContext";

interface CityPolicy {
  city: string;
  policyCount: number;
}

const romaniaCenter: LatLngExpression = [45.9432, 24.9668];

const normalize = (name: string) =>
  name.normalize("NFD").replace(/[\u0300-\u036f]/g, "").trim();

const getColor = (count: number) => {
  if (count > 50) return "#dc2626"; // red
  if (count > 20) return "#f97316"; // orange
  if (count > 10) return "#eab308"; // yellow
  return "#22c55e"; // green
};

function BrokerPoliciesMap() {
  const [data, setData] = useState<CityPolicy[]>([]);
  const [loading, setLoading] = useState(true);
  const { role } = useAuth();

 useEffect(() => {
  const fetchData = async () => {
    try {
      const endpoint =
        role === "Admin"
          ? "/admin/policies/reports/policy-by-city"
          : "/brokers/stats/policy-by-city";

      const res = await apiClient.get<CityPolicy[]>(endpoint);
      setData(res.data);

    } catch (err) {
      console.error("Error loading map data", err);
    } finally {
      setLoading(false);
    }
  };

  fetchData();
}, [role]);

  const topCities = [...data]
    .sort((a, b) => b.policyCount - a.policyCount)
    .slice(0, 5);

  if (loading) {
    return (
      <div className="h-125 bg-slate-200 animate-pulse rounded-2xl" />
    );
  }

  return (
    <div className="bg-white p-6 rounded-2xl shadow-md border border-slate-200 hover:shadow-lg transition">
      
      {/* HEADER */}
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-slate-800">
          Policies Distribution
        </h2>
        <p className="text-sm text-slate-500">
          Visual overview of your policies across Romania
        </p>
      </div>

      {/* GRID layout */}
      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">

        {/* MAP */}
        <div className="lg:col-span-3 relative">
          <MapContainer
            center={romaniaCenter}
            zoom={6}
            className="h-125 w-full rounded-xl z-0"
          >
            <TileLayer
              attribution="&copy; OpenStreetMap contributors"
              url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

            {data.map((city) => {
              const coords = cityCoordinates[normalize(city.city)];

              if (!coords) {
                console.warn("Missing coords for:", city.city);
                return null;
              }

              return (
                <CircleMarker
                  key={city.city}
                  center={coords}
                  radius={Math.min(city.policyCount * 2, 20)}
                  pathOptions={{
                    color: getColor(city.policyCount),
                    fillColor: getColor(city.policyCount),
                    fillOpacity: 0.7,
                  }}
                >
                  <Tooltip direction="top" offset={[0, -5]} opacity={1}>
                    {city.city}
                  </Tooltip>

                  <Popup>
                    <div className="text-sm">
                      <strong>{city.city}</strong>
                      <br />
                      Policies: {city.policyCount}
                    </div>
                  </Popup>
                </CircleMarker>
              );
            })}
          </MapContainer>

          <div className="absolute bottom-4 left-4 bg-white p-3 rounded-lg shadow text-xs z-1000">
            <div className="font-semibold mb-2">Policies</div>
            <div className="flex items-center gap-2">
              <span className="w-3 h-3 bg-green-500 rounded-full"></span> 0–10
            </div>
            <div className="flex items-center gap-2">
              <span className="w-3 h-3 bg-yellow-400 rounded-full"></span> 10–20
            </div>
            <div className="flex items-center gap-2">
              <span className="w-3 h-3 bg-orange-500 rounded-full"></span> 20–50
            </div>
            <div className="flex items-center gap-2">
              <span className="w-3 h-3 bg-red-600 rounded-full"></span> 50+
            </div>
          </div>
        </div>

        <div className="bg-slate-50 p-4 rounded-xl border border-slate-200">
          <h3 className="font-semibold mb-4 text-slate-700">
            Top Cities
          </h3>

          {topCities.map((c, index) => (
            <div
              key={c.city}
              className="flex justify-between items-center py-2 border-b last:border-none text-sm"
            >
              <div className="flex items-center gap-2">
                <span className="text-xs font-bold text-slate-400">
                  #{index + 1}
                </span>
                <span>{c.city}</span>
              </div>
              <span className="font-semibold text-slate-700">
                {c.policyCount}
              </span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default BrokerPoliciesMap;