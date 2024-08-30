import React, { useState, useEffect } from "react";
import {
  TextField,
  Button,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Typography,
  Paper,
  Grid,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import {
  getPermissionTypes,
  addPermissionType,
  removePermissionType,
  editPermissionType,
} from "../services/api";
import { PermissionType } from "../types/types";

const PermissionTypesManagement: React.FC = () => {
  const [permissionTypes, setPermissionTypes] = useState<PermissionType[]>([]);
  const [newTypeDescription, setNewTypeDescription] = useState("");
  const [editingType, setEditingType] = useState<PermissionType | null>(null);
  const [openDialog, setOpenDialog] = useState(false);

  useEffect(() => {
    fetchPermissionTypes();
  }, []);

  const fetchPermissionTypes = async () => {
    try {
      const response = await getPermissionTypes();
      setPermissionTypes(response.data);
    } catch (error) {
      console.error("Error fetching permission types:", error);
    }
  };

  const handleAddType = async () => {
    try {
      await addPermissionType({ description: newTypeDescription });
      setNewTypeDescription("");
      fetchPermissionTypes();
    } catch (error) {
      console.error("Error adding permission type:", error);
    }
  };

  const handleRemoveType = async (id: number) => {
    try {
      await removePermissionType(id);
      fetchPermissionTypes();
    } catch (error) {
      console.error("Error removing permission type:", error);
    }
  };

  const handleEditType = (type: PermissionType) => {
    setEditingType(type);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setEditingType(null);
    setOpenDialog(false);
  };

  const handleSaveEdit = async () => {
    if (editingType) {
      try {
        await editPermissionType(editingType.id, {
          id: editingType.id,
          description: editingType.description,
        });
        fetchPermissionTypes();
        handleCloseDialog();
      } catch (error) {
        console.error("Error editing permission type:", error);
      }
    }
  };

  return (
    <Paper elevation={3} sx={{ padding: 3, maxWidth: 600, margin: "auto" }}>
      <Typography variant='h4' gutterBottom>
        Manage Permission Types
      </Typography>
      <Grid container spacing={2} alignItems='center' sx={{ marginBottom: 3 }}>
        <Grid item xs={8}>
          <TextField
            fullWidth
            label='New Permission Type'
            value={newTypeDescription}
            onChange={(e) => setNewTypeDescription(e.target.value)}
          />
        </Grid>
        <Grid item xs={4}>
          <Button
            fullWidth
            onClick={handleAddType}
            variant='contained'
            color='primary'
            disabled={!newTypeDescription.trim()}
          >
            Add Type
          </Button>
        </Grid>
      </Grid>
      <List>
        {permissionTypes.map((type) => (
          <ListItem key={type.id} divider>
            <ListItemText primary={type.description} />
            <ListItemSecondaryAction>
              <IconButton
                edge='end'
                aria-label='edit'
                onClick={() => handleEditType(type)}
                sx={{ marginRight: 1 }}
              >
                <EditIcon />
              </IconButton>
              <IconButton
                edge='end'
                aria-label='delete'
                onClick={() => handleRemoveType(type.id)}
              >
                <DeleteIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>
      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Edit Permission Type</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin='dense'
            label='Description'
            fullWidth
            value={editingType?.description || ""}
            onChange={(e) =>
              setEditingType((prev) =>
                prev ? { ...prev, description: e.target.value } : null
              )
            }
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancel</Button>
          <Button onClick={handleSaveEdit} color='primary'>
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </Paper>
  );
};

export default PermissionTypesManagement;
