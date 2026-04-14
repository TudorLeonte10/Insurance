import apiClient from "./apiClient";

export const getPoliciesUnderReview = async () => {
    const res = await apiClient.get("/admin/");
    return res.data;
}