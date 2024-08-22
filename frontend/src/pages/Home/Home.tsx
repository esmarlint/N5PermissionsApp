import React from "react";
import { Typography, Box, Button } from "@mui/material";
import { Link } from "react-router-dom";

const Home: React.FC = () => {
  return (
    <Box sx={{ p: 3, textAlign: 'center' }}>
      <Typography variant="h2" component="h1" gutterBottom>
        Welcome to Permission Manager
      </Typography>
      <Typography variant="h5" component="p" gutterBottom>
        Manage your permissions efficiently.
      </Typography>
      <Button component={Link} to="/permissions" variant="contained" color="primary" sx={{ mt: 2 }}>
        View Permissions
      </Button>
    </Box>
  );
};

export default Home;