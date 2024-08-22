export interface PermissionDto {
  id: number;
  employeeName: string;
  employeeLastName: string;
  permissionTypeId: number;
  permissionDate?: string;
  permissionTypeName?: string;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  errorMessage: string | null;
  value: T;
}

export interface PermissionType {
  id: number;
  description: string;
}

export interface PermissionDtoPaginatedResult {
  items: PermissionDto[];
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
}

export interface RequestPermissionCommand {
  employeeName: string;
  employeeLastName: string;
  permissionTypeId: number;
}

export interface ModifyPermissionCommand {
  id: number;
  employeeName: string;
  employeeLastName: string;
  permissionTypeId: number;
}

export interface PermissionDto {
  id: number;
  employeeName: string;
  employeeLastName: string;
  permissionTypeId: number;
  permissionDate?: string;
  permissionTypeName?: string;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  errorMessage: string | null;
  value: T;
}
