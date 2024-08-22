import React from "react";
import { Link } from "react-router-dom";
import {
  Typography,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TablePagination,
  CircularProgress,
  Alert,
} from "@mui/material";
import usePermissions from "../../hooks/usePermissions";

const PermissionList: React.FC = () => {
  const {
    permissions,
    page,
    rowsPerPage,
    totalCount,
    loading,
    error,
    handleChangePage,
    handleChangeRowsPerPage,
  } = usePermissions();

  return (
    <div>
      <Typography variant='h4' gutterBottom>
        Permissions
      </Typography>
      <Button
        component={Link}
        to='/permissions/new'
        variant='contained'
        color='primary'
        sx={{ mb: 2 }}
      >
        New Permission
      </Button>
      {loading && <CircularProgress />}
      {error && <Alert severity='error'>{error}</Alert>}
      {!loading && !error && (
        <>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>ID</TableCell>
                  <TableCell>Employee Name</TableCell>
                  <TableCell>Employee Last Name</TableCell>
                  <TableCell>Permission Type</TableCell>
                  <TableCell>Date</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {permissions.map((permission) => (
                  <TableRow key={permission.id}>
                    <TableCell>{permission.id}</TableCell>
                    <TableCell>{permission.employeeName}</TableCell>
                    <TableCell>{permission.employeeLastName}</TableCell>
                    <TableCell>{permission.permissionTypeName}</TableCell>
                    <TableCell>
                      {new Date(
                        permission.permissionDate!
                      ).toLocaleDateString()}
                    </TableCell>
                    <TableCell>
                      <Button
                        component={Link}
                        to={`/permissions/edit/${permission.id}`}
                        size='small'
                      >
                        Edit
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <TablePagination
            component='div'
            count={totalCount}
            page={page}
            onPageChange={handleChangePage}
            rowsPerPage={rowsPerPage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </>
      )}
    </div>
  );
};

export default PermissionList;
