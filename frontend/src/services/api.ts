import axios, { AxiosResponse } from "axios";
import {
  PermissionDtoPaginatedResult,
  ApiResponse,
  PermissionDto,
  PermissionType,
} from "../types/types";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const getPermissions = (
  pageNumber: number = 1,
  pageSize: number = 10
): Promise<AxiosResponse<PermissionDtoPaginatedResult>> =>
  api.get(`/permissions`, {
    params: { PageNumber: pageNumber, PageSize: pageSize },
  });

export const getPermissionById = (
  id: number
): Promise<AxiosResponse<ApiResponse<PermissionDto>>> =>
  api.get(`/permissions/${id}`);

export const modifyPermission = (
  id: number,
  data: Partial<PermissionDto>
): Promise<AxiosResponse<void>> => api.put(`/permissions/${id}`, data);

export const requestPermission = (
  data: Omit<PermissionDto, "id" | "permissionDate" | "permissionTypeName">
): Promise<AxiosResponse<void>> => api.post(`/permissions`, data);

export const getPermissionTypes = (): Promise<
  AxiosResponse<PermissionType[]>
> => api.get("/permission-types");

export const addPermissionType = (data: {
  description: string;
}): Promise<AxiosResponse<number>> => api.post("/permission-types", data);

export const editPermissionType = (
  id: number,
  data: { id: number; description: string }
): Promise<AxiosResponse<void>> => api.put(`/permission-types/${id}`, data);

export const removePermissionType = (
  id: number
): Promise<AxiosResponse<void>> => api.delete(`/permission-types/${id}`);

export default api;
