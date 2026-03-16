import { Outlet, Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

function BrokerLayout() {
    const { logout } = useAuth();

    return (
    <div className="flex min-h-screen bg-gray-100">

      <div className="w-64 bg-white shadow-md p-5 flex flex-col justify-between">

        <div>
          <h1 className="text-xl font-bold mb-6">Insurance</h1>

          <nav className="flex flex-col gap-3">
            <Link
              to="/broker/dashboard"
              className="hover:bg-gray-200 p-2 rounded"
            >
              Dashboard
            </Link>

            <Link
              to="/broker/clients"
              className="hover:bg-gray-200 p-2 rounded"
            >
              Clients
            </Link>

            <Link
              to="/broker/policies"
              className="hover:bg-gray-200 p-2 rounded"
            >
              Policies
            </Link>
          </nav>
        </div>

        <button
          onClick={logout}
          className="bg-red-500 text-white p-2 rounded"
        >
          Logout
        </button>

      </div>

      <main className="flex-1 p-6">
        <Outlet />
      </main>

    </div>
  );
}


export default BrokerLayout