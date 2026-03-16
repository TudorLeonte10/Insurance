import apiClient from "./apiClient";

export interface Client {
    id: number;
    name: string;
    identificationNumber: string;
    type: string;
    email: string;
    phoneNumber: string;
    address: string;
}

interface PagedResult<T> {
    items: T[];
}

export const getBrokerClients = async (): Promise<Client[]> => {
    const response = await apiClient.get<PagedResult<Client>>("/brokers/clients/clients");
    return response.data.items;
}