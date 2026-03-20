import { jwtDecode } from "jwt-decode";
import { createContext, useContext, useState, useEffect } from "react";

interface AuthContextType {
  token: string | null;
  role: string | null;
  username: string | null;
  login: (token: string, role: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {

  const [token, setToken] = useState<string | null>(null);
  const [role, setRole] = useState<string | null>(null);
  const [username, setUsername] = useState<string | null>(null);

  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const storedUsername = localStorage.getItem("username");

    if (storedToken) {
      setToken(storedToken);
      setUsername(storedUsername);
    }
  }, []);

  const login = (token: string, role: string) => {

    const decoded: any = jwtDecode(token);

    console.log("JWT decoded:", decoded);

    const extractedUsername =
      decoded["username"] ||
      decoded["unique_name"] ||
      decoded["name"] ||
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

    localStorage.setItem("token", token);
    localStorage.setItem("username", extractedUsername);
    localStorage.setItem("role", role);

    setToken(token);
    setRole(role);
    setUsername(extractedUsername); 
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");

    setToken(null);
    setRole(null);
    setUsername(null);
  };

  return (
    <AuthContext.Provider value={{ token, role, username, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
}