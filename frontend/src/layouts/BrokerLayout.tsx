import { Outlet, Link, useLocation } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { useState } from "react";
import { LayoutDashboard, Users, FileText, LogOut, Menu, X } from "lucide-react";
import { useNavigate } from "react-router-dom";

function BrokerLayout() {
  const navigate = useNavigate();
  const { logout, username } = useAuth();
  const location = useLocation();
  const [isOpen, setIsOpen] = useState(false);

  const navItems = [
    { name: "Dashboard", path: "/broker/dashboard", icon: <LayoutDashboard size={18} /> },
    { name: "Clients", path: "/broker/clients", icon: <Users size={18} /> },
    { name: "Policies", path: "/broker/policies", icon: <FileText size={18} /> }
  ];

  const isActivePath = (path: string) => location.pathname.startsWith(path);

  return (
    <div className="flex h-screen bg-slate-50 overflow-hidden text-slate-900">
      
      {/* MOBILE OVERLAY */}
      {isOpen && (
        <div 
          className="fixed inset-0 bg-black/40 backdrop-blur-sm z-40 md:hidden"
          onClick={() => setIsOpen(false)}
        />
      )}

      {/* SIDEBAR */}
      <aside
        className={`
          fixed md:static z-50 h-full w-64 bg-slate-900 text-white flex flex-col
          transform transition-transform duration-300 ease-in-out
          ${isOpen ? "translate-x-0" : "-translate-x-full md:translate-x-0"}
        `}
      >
        {/* LOGO */}
        <div className="flex items-center justify-between px-6 py-5 border-b border-white/[0.06]">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-teal-600 flex items-center justify-center">
              <span className="text-xs font-bold tracking-tight">IP</span>
            </div>
            <span className="text-[15px] font-semibold tracking-tight">InsurePro</span>
          </div>
          <button 
            onClick={() => setIsOpen(false)} 
            className="md:hidden p-1 hover:bg-white/10 rounded"
          >
            <X size={18} />
          </button>
        </div>

        {/* NAVIGATION */}
        <nav className="flex-1 px-3 py-4 space-y-0.5">
          <p className="text-[10px] font-semibold uppercase tracking-widest text-slate-500 px-3 mb-3">
            Menu
          </p>
          {navItems.map((item) => {
            const isActive = isActivePath(item.path);
            return (
              <Link
                key={item.path}
                to={item.path}
                onClick={() => setIsOpen(false)}
                className={`
                  flex items-center gap-3 px-3 py-2.5 rounded-lg text-[13px] transition-all duration-150
                  ${isActive 
                    ? "bg-teal-600/15 text-teal-400 font-medium" 
                    : "text-slate-400 hover:bg-white/[0.05] hover:text-slate-200"}
                `}
              >
                <span className={isActive ? "text-teal-400" : "text-slate-500"}>{item.icon}</span>
                <span>{item.name}</span>
                {isActive && <span className="ml-auto w-1.5 h-1.5 rounded-full bg-teal-400" />}
              </Link>
            );
          })}
        </nav>

        {/* USER PROFILE & LOGOUT */}
        <div className="px-3 py-4 border-t border-white/[0.06]">
          <div className="flex items-center gap-3 px-3 mb-3">
            <div className="w-8 h-8 rounded-full bg-slate-700 flex items-center justify-center text-xs font-semibold text-slate-300">
              {username?.charAt(0).toUpperCase()}{username?.charAt(1)?.toUpperCase()}
            </div>
            <div className="overflow-hidden">
              <p className="text-[13px] font-medium text-slate-200 truncate">{username}</p>
              <p className="text-[11px] text-slate-500 truncate">Broker</p>
            </div>
          </div>
          
          <button
            onClick={() => {
              logout();
              navigate("/");
            }}
            className="w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-[13px] text-slate-500 hover:bg-red-500/10 hover:text-red-400 transition-colors"
          >
            <LogOut size={16} />
            <span>Sign out</span>
          </button>
        </div>
      </aside>

      {/* MAIN CONTENT AREA */}
      <div className="flex-1 flex flex-col min-w-0 overflow-hidden">
        
        {/* MOBILE HEADER */}
        <header className="md:hidden flex items-center justify-between px-4 py-3 bg-white border-b border-slate-200/80">
          <button onClick={() => setIsOpen(true)} className="p-2 -ml-2 hover:bg-slate-100 rounded-lg">
            <Menu size={20} />
          </button>
          <span className="text-sm font-semibold text-slate-900">InsurePro</span>
          <div className="w-8" />
        </header>

        {/* SCROLLABLE CONTENT */}
        <main className="flex-1 overflow-y-auto custom-scrollbar">
          <div className="max-w-[1200px] mx-auto px-4 md:px-8 py-6 md:py-8">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}

export default BrokerLayout;