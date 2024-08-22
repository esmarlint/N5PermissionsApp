import { useState, useEffect } from "react";
import { getPermissions } from "../services/api";
import { PermissionDto } from "../types/types";

const usePermissions = (initialPage = 0, initialRowsPerPage = 10) => {
  const [permissions, setPermissions] = useState<PermissionDto[]>([]);
  const [page, setPage] = useState(initialPage);
  const [rowsPerPage, setRowsPerPage] = useState(initialRowsPerPage);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPermissions = async () => {
      setLoading(true);
      setError(null);
      try {
        const response = await getPermissions(page + 1, rowsPerPage);
        setPermissions(response.data.items);
        setTotalCount(response.data.totalCount);
      } catch (err) {
        setError("Error fetching permissions");
        console.error("Error fetching permissions:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchPermissions();
  }, [page, rowsPerPage]);

  const handleChangePage = (_: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  return {
    permissions,
    page,
    rowsPerPage,
    totalCount,
    loading,
    error,
    handleChangePage,
    handleChangeRowsPerPage,
  };
};

export default usePermissions;
