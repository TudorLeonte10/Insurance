import { BuildingType, RiskIndicator } from "../types/buildings";
import apiClient from "./apiClient";

export interface CreateBuildingDto {
    street: string;
    number: string;
    constructionYear: number;
    insuredValue: number;
    numberOfFloors: number;
    surfaceArea: number;
    cityId: string;
    type: BuildingType;
    riskIndicators: RiskIndicator[];
}

export const createBuilding = async (
  clientId: string,
  building: CreateBuildingDto
) => {

  const response = await apiClient.post(
    `brokers/clients/${clientId}/buildings`,
    building
  );

  return response.data;
};

export const getBuildingsByClient = async (clientId: string) => {
  const res = await apiClient.get(
    `/brokers/clients/${clientId}/buildings`
  );
  return res.data;
};