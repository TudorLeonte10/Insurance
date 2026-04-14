import { Outlet, Link, useLocation } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { useState } from "react";
import { 
  LayoutDashboard, 
  Users, 
  Database, 
  BarChart3, 
  LogOut, 
  Menu,
  ShieldCheck
} from "lucide-react";

function AdminLayout() {
  const { logout, username } = useAuth();
  const location = useLocation();
  const [isOpen, setIsOpen] = useState(false);

  const navItems = [
    { name: "Dashboard", path: "/admin/dashboard", icon: <LayoutDashboard size={20} /> },
    { name: "Brokers", path: "/admin/brokers", icon: <Users size={20} /> },
    { name: "Policies Review", path: "/admin/review-policies", icon: <ShieldCheck size={20} /> },
    { name: "Metadata", path: "/admin/metadata", icon: <Database size={20} /> },
    { name: "Reports", path: "/admin/reports", icon: <BarChart3 size={20} /> },

  ];

  return (
    <div className="flex h-screen bg-[#f8fafc] overflow-hidden text-slate-900">

      {/* MOBILE OVERLAY */}
      {isOpen && (
        <div 
          className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm z-40 md:hidden"
          onClick={() => setIsOpen(false)}
        />
      )}

      {/* SIDEBAR */}
      <aside
        className={`
          fixed md:static z-50 h-full w-72 bg-[#001a3d] text-white p-6 flex flex-col
          transform transition-all duration-300 ease-in-out border-r border-white/10
          ${isOpen ? "translate-x-0" : "-translate-x-full md:translate-x-0"}
        `}
      >
        {/* LOGO */}
        <div className="flex items-center gap-3 mb-10 px-2">
          <div className="bg-indigo-600 p-2 rounded-lg">
            <ShieldCheck size={24} className="text-white" />
          </div>
          <span className="text-xl font-bold tracking-tight">InsurePro Admin</span>
        </div>

        {/* NAVIGATION */}
        <nav className="flex-1 space-y-1">
          {navItems.map((item) => {
            const isActive = location.pathname === item.path;
            return (
              <Link
                key={item.path}
                to={item.path}
                onClick={() => setIsOpen(false)}
                className={`
                  flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200
                  ${isActive 
                    ? "bg-indigo-600 text-white shadow-lg shadow-indigo-900/20" 
                    : "text-slate-300 hover:bg-white/10 hover:text-white"}
                `}
              >
                {item.icon}
                <span className="font-medium">{item.name}</span>
              </Link>
            );
          })}
        </nav>

        {/* USER PROFILE & LOGOUT */}
        <div className="mt-auto pt-6 border-t border-white/10">
          <div className="flex items-center gap-3 px-2 mb-4">
            <div className="w-10 h-10 rounded-full bg-slate-500 flex items-center justify-center text-sm font-bold border-2 border-white/20">
              {username?.charAt(0).toUpperCase()}
              {username?.charAt(1).toUpperCase()}
            </div>
            <div className="overflow-hidden">
              <p className="text-sm font-medium truncate">{username}</p>
              <p className="text-xs text-slate-400 truncate">Administrator</p>
            </div>
          </div>

          <button
            onClick={logout}
            className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-red-400 hover:bg-red-500/10 hover:text-red-300 transition-colors"
          >
            <LogOut size={20} />
            <span className="font-medium">Logout</span>
          </button>
        </div>
      </aside>

      {/* MAIN CONTENT */}
      <div className="flex-1 flex flex-col min-w-0 overflow-hidden">

        {/* MOBILE HEADER */}
        <header className="md:hidden flex items-center justify-between p-4 bg-white border-b border-slate-200">
          <button onClick={() => setIsOpen(true)} className="p-2 hover:bg-slate-100 rounded-lg">
            <Menu size={24} />
          </button>
          <span className="font-bold text-[#001a3d]">Admin Panel</span>
          <div className="w-8" />
        </header>

        {/* CONTENT */}
        <main className="flex-1 overflow-y-auto p-4 md:p-8 custom-scrollbar">
          <div className="max-w-6xl mx-auto">

            {/* HEADER */}
            <header className="mb-8 hidden md:block">
              <h2 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-1">
                Admin Control Panel
              </h2>
              <p className="text-slate-500">
                Monitor system activity, manage brokers, and analyze performance.
              </p>
            </header>

            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}

export default AdminLayout;