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

export default api;
