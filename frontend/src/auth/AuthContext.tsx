import {createContext, useContext, useState } from 'react'

interface AuthContextType
{
    token : string | null,
    role : string | null,
    login : (token: string, role: string) => void,
    logout : () => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({children}:{children: React.ReactNode}){
    const[token, setToken] = useState<string | null>(null);
    const[role, setRole] = useState<string | null>(null);

    const login = (token: string, role: string) => {
        localStorage.setItem("token", token);
        setToken(token);
        setRole(role);
    }

    const logout = () => {
        localStorage.removeItem("token");
        setToken(null);
        setRole(null);
    }

    return (
        <AuthContext.Provider value ={{token, role, login, logout}}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth(){
    const context = useContext(AuthContext);
    if(!context){
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
}
    