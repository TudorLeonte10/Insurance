
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

export interface Broker {
    id: number;
    brokerCode: string;
    name: string;
    email: string;
    phone: string;
    isActive: boolean;
}

export interface CreateBrokerDto {
    brokerCode: string;
    name: string;
    email: string;
    phone: string;
    isActive: boolean;
}

export interface UpdateBrokerDto {
    name: string;
    email: string;
    phone: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
}

export const getBrokerClients = async (
  page: number,
  pageSize: number,
  search?: string
): Promise<PagedResult<Client>> => {

  const response = await apiClient.get<PagedResult<Client>>(
    `/brokers/clients/clients?pageNumber=${page}&pageSize=${pageSize}&name=${search ?? ""}`
  );

  return response.data;
};

export const getAllBrokers = async (pageNumber: number, pageSize: number) : Promise<PagedResult<Broker>> => {
  const response = await apiClient.get<PagedResult<Broker>>(
    `/admin/brokers?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );

  return response.data;
};

export const getBrokerById = async (brokerId: string) => {
  const response = await apiClient.get(`/admin/brokers/${brokerId}`);
  return response.data;
}

export const activateBroker = async (brokerId: string) => {
    const response = await apiClient.patch(`/admin/brokers/${brokerId}/activate`);
    return response.data;
};

export const deactivateBroker = async (brokerId: string) => {
    const response = await apiClient.patch(`/admin/brokers/${brokerId}/deactivate`);
    return response.data;
};

export const createBroker = async (broker: CreateBrokerDto) => {
    const response = await apiClient.post(`/admin/brokers`, broker);
    return response.data;
};

export const updateBroker = async (broker: UpdateBrokerDto, brokerId: string) => {
    const response = await apiClient.put(`/admin/brokers/${brokerId}`, broker);
    return response.data;
};
