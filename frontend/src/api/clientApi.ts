import apiClient from "./apiClient";
import type { ClientType } from "../types/clients";


export interface CreateClientDto {
  name: string;
  identificationNumber: string;
  email: string;
  phoneNumber: string;
  address: string;
  type: ClientType;
}

export interface UpdateClientDto {
  name: string;
  email: string;
  phoneNumber: string;
  address: string;
  identificationNumber: string;
}

export const createClient = async(
    client: CreateClientDto) => {
        const response = await apiClient.post("/brokers/clients", client);
        return response.data;
    }

export const getClientById = async (id: string) => {
    const response = await apiClient.get(`/brokers/clients/${id}`);
    return response.data;
};

export const updateClient = async (id: string, client: UpdateClientDto) => {
    const response = await apiClient.put(`/brokers/clients/${id}`, client);
    return response.data;
}

export const getClients = async (pageNumber: number, pageSize: number) => {
    const response = await apiClient.get(`/brokers/clients/clients?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return response.data;
}