import apiClient from "./apiClient";

export interface PolicyQueryParams {
  clientId?: string;
  status?: string;
  startDateFrom?: string;
  startDateTo?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface CreatePolicyDto{
    clientId: string;
    buildingId: string;
    currencyId: string;
    basePremium: number;
    startDate: string;
    endDate: string;
}

export const getPolicies = async (params: PolicyQueryParams) => {
    const res = await apiClient.get("/brokers/policies", { params });
    return res.data;
};

export const getPolicyById = async (policyId: string) => {
    const res = await apiClient.get(`/brokers/policies/${policyId}`);
    return res.data;
}

export const createPolicy = async (policy: CreatePolicyDto) => {
  const res = await apiClient.post("/brokers/policies", policy);

  const locationHeader = res.headers.location;

  if (!locationHeader) {
    throw new Error("Location header missing");
  }

  const id = locationHeader.split("/").pop();

  return { id };
};

export const activatePolicy = async (policyId: string) => {
    const res = await apiClient.post(`/brokers/policies/${policyId}/activate`);
    return res.data;
}

export const cancelPolicy = async (policyId: string) => {
    const res = await apiClient.post(`/brokers/policies/${policyId}/cancel`);
    return res.data;
}