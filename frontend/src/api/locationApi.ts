import apiClient from "./apiClient";

export interface Country {
  id: string;
  name: string;
}

export interface County {
  id: string;
  name: string;
}

export interface City {
  id: string;
  name: string;
}

export const getCountries = async () => {
  const response = await apiClient.get("/brokers/countries");
  return response.data;
};

export const getCounties = async (countryId: string) => {
  const response = await apiClient.get(`/brokers/countries/${countryId}/counties`);
  return response.data;
};

export const getCities = async (countyId: string) => {
  const response = await apiClient.get(`/brokers/countries/${countyId}/cities`);
  return response.data;
};