import apiClient from "./apiClient";
import { FeeType, RiskFactorLevel } from "../types/metadata";

interface CreateCurrencyDto{
    code: string;
    name: string;
    exchangeRateToBase: number;
}

interface CreateFeeConfigurationDto {
    name: string;
    type: FeeType;
    percentage: number;
    effectiveFrom: string;
    effectiveTo?: string;
}

interface UpdateFeeConfigurationDto {
    name?: string;
    type?: FeeType;
    percentage?: number;
    effectiveFrom?: string;
    effectiveTo?: string;
    isActive?: boolean;
}

interface CreateRiskFactorDto {
    level: RiskFactorLevel;
    referenceId: string;
    adjustmentPercentage: number;
}

interface UpdateRiskFactorDto {
    level?: RiskFactorLevel;
    referenceId?: string;
    adjustmentPercentage?: number;
    isActive?: boolean;
}

export const getCurrencies = async () => {
    const res = await apiClient.get("/currencies");
    return res.data;
}

export const createCurrency = async (currency: CreateCurrencyDto) => {
    const res = await apiClient.post("/currencies", currency);
    return res.data;
}

export const getFeeConfiguration = async () => {
    const res = await apiClient.get("/admin/fees");
    return res.data;
}

export const createFeeConfiguration = async (feeConfig: CreateFeeConfigurationDto) => {
    const res = await apiClient.post("/admin/fees", feeConfig);
    return res.data;
}

export const updateFeeConfiguration = async (id: string, feeConfig: UpdateFeeConfigurationDto) => {
    const res = await apiClient.put(`/admin/fees/${id}`, feeConfig);
    return res.data;
}

export const getAllRiskFactors = async () => {
    const res = await apiClient.get("/admin/risk-factors");
    return res.data;
}

export const createRiskFactor = async (riskFactor: CreateRiskFactorDto) => {
    const res = await apiClient.post("/admin/risk-factors", riskFactor);
    return res.data;
}

export const updateRiskFactor = async (id: string, riskFactor: UpdateRiskFactorDto) => {
    const res = await apiClient.put(`/admin/risk-factors/${id}`, riskFactor);
    return res.data;
}
