import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import ClientsPage from "./pages/ClientsPage";
import BrokerLayout from "./layouts/BrokerLayout";
import AdminLayout from "./layouts/AdminLayout";
import ClientDetailsPage from "./pages/ClientDetailsPage";
import CreateBuilldingPage from "./pages/CreateBuildingPage";
import CreateClientPage from "./pages/CreateClientPage";
import { useAuth } from "./auth/AuthContext";
import EditClientPage from "./pages/EditClientPage";
import BrokersPage from "./pages/BrokersPage";
import CreateBrokerPage from "./pages/CreateBrokerPage";
import EditBrokerPage from "./pages/EditBrokerPage";
import BrokerPoliciesPage from "./pages/PoliciesPage";
import PolicyDetailsPage from "./pages/ViewPolicyPage";
import CreatePolicyPage from "./pages/CreatePolicyPage";

function App() {
  const { role } = useAuth();

  return (
    <Routes>

      <Route path="/" element={<LoginPage />} />

      <Route
        path="/broker"
        element={role === "Broker" ? <BrokerLayout /> : <Navigate to="/" />}
        //element={<BrokerLayout />}
      >
        <Route index element={<Navigate to="dashboard" />} />
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="clients" element={<ClientsPage />} />
        <Route path="clients/:id" element={<ClientDetailsPage />} />
        <Route path="clients/:id/buildings/create" element={<CreateBuilldingPage />} />
        <Route path="clients/create" element={<CreateClientPage />} />
        <Route path="clients/:id/edit" element={<EditClientPage />} />
        <Route path="policies" element={<BrokerPoliciesPage />} />
        <Route path="policies/create" element={<CreatePolicyPage />} />
        <Route path="policies/:id" element={<PolicyDetailsPage />} />
      </Route>

      <Route
        path="/admin"
        element={role === "Admin" ? <AdminLayout /> : <Navigate to="/" />}
        //element={<AdminLayout />}
      >
        <Route index element={<Navigate to="dashboard" />} />
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="brokers" element={<BrokersPage />} />
        <Route path="brokers/create" element={<CreateBrokerPage />} />
        <Route path="brokers/:id/edit" element={<EditBrokerPage />} />
        <Route path="reports" element={<div>Reports coming soon</div>} />
        <Route path="metadata" element={<div>Metadata coming soon</div>} />
      </Route>

    </Routes>
  );
}

export default App;