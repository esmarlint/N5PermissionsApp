import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
  getPermissionById,
  modifyPermission,
  requestPermission,
  getPermissionTypes,
} from "../services/api";
import { PermissionType, PermissionDto } from "../types/types";

const usePermissionForm = (id?: string) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<Partial<PermissionDto>>({
    employeeName: "",
    employeeLastName: "",
    permissionTypeId: 0,
  });
  const [permissionTypes, setPermissionTypes] = useState<PermissionType[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const typesResponse = await getPermissionTypes();
        setPermissionTypes(typesResponse.data);

        if (id) {
          const permissionResponse = await getPermissionById(parseInt(id));
          if (
            permissionResponse.data.isSuccess &&
            permissionResponse.data.value
          ) {
            setFormData(permissionResponse.data.value);
          } else {
            console.error(
              "Failed to fetch permission:",
              permissionResponse.data.errorMessage
            );
          }
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "permissionTypeId" ? parseInt(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (id) {
        await modifyPermission(parseInt(id), formData as PermissionDto);
      } else {
        await requestPermission(
          formData as Omit<
            PermissionDto,
            "id" | "permissionDate" | "permissionTypeName"
          >
        );
      }
      navigate("/permissions");
    } catch (error) {
      console.error("Error saving permission:", error);
    }
  };

  return {
    formData,
    permissionTypes,
    loading,
    handleChange,
    handleSubmit,
  };
};

export default usePermissionForm;
