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
  if (count > 50) return "#b91c1c";
  if (count > 20) return "#c2410c";
  if (count > 10) return "#a16207";
  return "#0d9488";
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
    <div className="bg-white p-6 rounded-xl border border-slate-200/80">
      
      {/* HEADER */}
      <div className="mb-6">
        <h2 className="text-base font-semibold text-slate-900">
          Policy Distribution
        </h2>
        <p className="text-sm text-slate-500 mt-0.5">
          Geographic overview across Romania
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

          <div className="absolute bottom-4 left-4 bg-white/95 backdrop-blur-sm p-3 rounded-lg border border-slate-200 text-xs z-1000">
            <div className="font-semibold text-slate-700 mb-2">Policies</div>
            <div className="flex items-center gap-2 text-slate-600">
              <span className="w-2.5 h-2.5 rounded-full" style={{ background: "#0d9488" }}></span> 0 – 10
            </div>
            <div className="flex items-center gap-2 text-slate-600">
              <span className="w-2.5 h-2.5 rounded-full" style={{ background: "#a16207" }}></span> 10 – 20
            </div>
            <div className="flex items-center gap-2 text-slate-600">
              <span className="w-2.5 h-2.5 rounded-full" style={{ background: "#c2410c" }}></span> 20 – 50
            </div>
            <div className="flex items-center gap-2 text-slate-600">
              <span className="w-2.5 h-2.5 rounded-full" style={{ background: "#b91c1c" }}></span> 50+
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
              className="flex justify-between items-center py-2 border-b border-slate-200 last:border-none text-sm"
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