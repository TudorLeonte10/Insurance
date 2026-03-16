import { Outlet } from 'react-router-dom'

function AdminLayout() {
    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold mb-4">Admin Dashboard</h1>
            <p>Welcome to the admin dashboard! Here you can manage your insurance policies and clients.</p>
            <Outlet />
        </div>
    )
}

export default AdminLayout