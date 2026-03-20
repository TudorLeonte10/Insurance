import apiClient from "./apiClient";

export const getCurrencies = async () => {
  const response = await apiClient.get("/currencies");
  return response.data;
} 