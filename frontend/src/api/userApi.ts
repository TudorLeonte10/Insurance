import type { LoaderFunction } from "react-router-dom";
import apiClient from "./apiClient";

export interface LoginRequest {
    username: string;
    password: string;
}

export interface LoginResponse {
    accessToken: string;
    role: string;
}

export const login = async (creds: LoginRequest): Promise<LoginResponse> => {
    const response = await apiClient.post("/auth/login", creds);
    return response.data;
}

