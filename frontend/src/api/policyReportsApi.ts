import apiClient from "./apiClient";

export interface PolicySummaryDto {
  totalPolicies: number;
  totalPremium: number;
  averagePremium: number;
  activePolicies: number;
  underReviewPolicies: number;
}

export interface PolicyTimeseriesDto {
  date: string;
  policyCount: number;
  totalPremium: number;
}

export interface PolicyReportDto {
  groupName: string;
  currency: string;
  policiesCount: number;
  totalPremium: number;
  totalPremiumInBase: number;
}

export type ReportGroupingType = "Country" | "County" | "City" | "Broker" | "Status" | "BuildingType";

export interface ReportFilters {
  from?: string;
  to?: string;
  status?: string;
  currency?: string;
  buildingType?: string;
}

export const getPolicySummary = async (filters: ReportFilters): Promise<PolicySummaryDto> => {
  const res = await apiClient.get("/admin/policies/reports/summary", { params: filters });
  return res.data;
};

export const getPolicyTimeseries = async (filters: ReportFilters): Promise<PolicyTimeseriesDto[]> => {
  const params: Record<string, string> = {};
  if (filters.from) params.from = filters.from;
  if (filters.to) params.to = filters.to;
  if (filters.status) params.status = filters.status;
  if (filters.currency) params.currency = filters.currency;
  if (filters.buildingType) params.buildingType = filters.buildingType;
  const res = await apiClient.get("/admin/policies/reports/timeseries", { params });
  return res.data;
};

export const getPoliciesReport = async (
  groupingType: ReportGroupingType,
  filters: ReportFilters
): Promise<PolicyReportDto[]> => {
  const to = filters.to ?? new Date().toISOString();
  const from = filters.from ?? new Date(Date.now() - 364 * 24 * 60 * 60 * 1000).toISOString();
  const params: Record<string, string> = {
    reportGroupingType: groupingType,
    from,
    to,
  };
  if (filters.status) params.status = filters.status;
  if (filters.currency) params.currency = filters.currency;
  if (filters.buildingType) params.buildingType = filters.buildingType;
  const res = await apiClient.get("/admin/policies/reports", { params });
  return res.data;
};
