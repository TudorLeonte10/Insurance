import { useState } from "react";
import { useAuth } from "../auth/AuthContext";
import { useNavigate } from "react-router-dom";
import { Eye, EyeOff, Loader2 } from "lucide-react";
import { login as loginRequest } from "../api/userApi";
import { jwtDecode } from "jwt-decode";

function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null)

  const handleLogin = async () => {
  console.log("Attempting login with:", { username, password });
  setLoading(true);

  try {
    const response = await loginRequest({ username, password });

    const token = response.accessToken;
    const decodedToken: any = jwtDecode(token);
    console.log("DECODED TOKEN:", decodedToken);
    const role =
      decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    console.log("EXTRACTED ROLE:", role);

    console.log("LOGIN RESPONSE:", response);

    login(response.accessToken, role);

    if (role === "Admin") {
      navigate("/admin");
    } else {
      navigate("/broker/dashboard");
    }
  } catch (error : any) {
    setError("Login failed. Please check your credentials and try again.");
  } finally {
    setLoading(false);
  }
};

  return (
    <div
      className="min-h-screen flex items-center justify-center relative bg-cover bg-center bg-fixed"
      style={{
        backgroundImage:
          "url('https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&w=2200&q=80')",
      }}
    >
      {/* dark overlay */}
      <div className="absolute inset-0 bg-[#00204a]/70"></div>

      {/* animated light reflection */}
      <div className="absolute inset-0 overflow-hidden">
        <div className="light-sweep"></div>
      </div>

      {/* LOGIN CARD */}
      <div className="relative z-10 w-full max-w-md bg-white/90 backdrop-blur-lg border border-white/30 rounded-2xl p-10 shadow-[0_30px_80px_rgba(0,0,0,0.35)]">

        {/* Logo */}
        <div className="text-center mb-10">
          <h1 className="text-2xl font-semibold tracking-tight text-gray-900">
            INSURE<span className="text-[#00204a]">PRO</span>
          </h1>

          <p className="text-sm text-gray-500 mt-2">
            Secure platform access
          </p>
        </div>

        <div className="space-y-6">

          {/* USERNAME */}
          <div>
            <label className="text-sm text-gray-600 block mb-1">
              Username
            </label>

            <input
              type="text"
              placeholder="john.doe"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="w-full border border-gray-300 rounded-md px-4 py-3 focus:outline-none focus:ring-2 focus:ring-[#00204a]"
            />
          </div>

          {/* PASSWORD */}
          <div className="relative">
            <label className="text-sm text-gray-600 block mb-1">
              Password
            </label>

            <input
              type={showPassword ? "text" : "password"}
              placeholder="••••••••"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full border border-gray-300 rounded-md px-4 py-3 pr-10 focus:outline-none focus:ring-2 focus:ring-[#00204a]"
            />
            
            {error && <div className="text-red-600 text-sm">{error}</div>}

            <button
              type="button"
              className="absolute right-3 top-9 text-gray-400 hover:text-gray-700"
              onClick={() => setShowPassword(!showPassword)}
            >
              {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>

          {/* LOGIN BUTTON */}
          <button
            onClick={handleLogin}
            disabled={loading}
            className="w-full bg-[#00204a] text-white py-3 rounded-md flex items-center justify-center gap-2 hover:opacity-90 transition"
          >
            {loading && <Loader2 className="animate-spin" size={18} />}
            {loading ? "Signing in..." : "Sign In"}
          </button>

        </div>
      </div>
    </div>
  );
 }

export default LoginPage;