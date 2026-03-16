import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import ClientsPage from "./pages/ClientsPage";
import BrokerLayout from "./layouts/BrokerLayout";
import AdminLayout from "./layouts/AdminLayout";
import ClientDetailsPage from "./pages/ClientDetailsPage";
import CreateBuidingPage from "./pages/CreateBuildingPage";
import { useAuth } from "./auth/AuthContext";

function App() {
  const { role } = useAuth();

  return (
    <Routes>

      <Route path="/" element={<LoginPage />} />

      <Route
        path="/broker"
        element={role === "Broker" ? <BrokerLayout /> : <Navigate to="/" />}
      >
        <Route index element={<Navigate to="dashboard" />} />
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="clients" element={<ClientsPage />} />
        <Route path="clients/:id" element={<ClientDetailsPage />} />
        <Route path="clients/:id/buildings/create" element={<div>Create Building</div>} />
        <Route path="policies" element={<div>Policies coming soon</div>} />
      </Route>

      <Route
        path="/admin"
        element={role === "Admin" ? <AdminLayout /> : <Navigate to="/" />}
      />

    </Routes>
  );
}

export default App;