import React from "react";
import {
  TextField,
  Button,
  MenuItem,
  Box,
  CircularProgress,
} from "@mui/material";
import usePermissionForm from "../hooks/usePermissionForm";
import { useParams } from "react-router-dom";

const PermissionForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { formData, permissionTypes, loading, handleChange, handleSubmit } =
    usePermissionForm(id);

  if (loading) {
    return <CircularProgress />;
  }

  return (
    <Box
      component='form'
      onSubmit={handleSubmit}
      sx={{ "& .MuiTextField-root": { m: 1, width: "25ch" } }}
    >
      <TextField
        label='Employee Name'
        name='employeeName'
        value={formData.employeeName || ""}
        onChange={handleChange}
        required
      />
      <TextField
        label='Employee Last Name'
        name='employeeLastName'
        value={formData.employeeLastName || ""}
        onChange={handleChange}
        required
      />
      <TextField
        select
        label='Permission Type'
        name='permissionTypeId'
        value={formData.permissionTypeId || ""}
        onChange={handleChange}
        required
      >
        {permissionTypes.map((type) => (
          <MenuItem key={type.id} value={type.id}>
            {type.description}
          </MenuItem>
        ))}
      </TextField>
      <Button
        type='submit'
        variant='contained'
        color='primary'
        sx={{ marginTop: 2 }}
      >
        {id ? "Update" : "Create"} Permission
      </Button>
    </Box>
  );
};

export default PermissionForm;
