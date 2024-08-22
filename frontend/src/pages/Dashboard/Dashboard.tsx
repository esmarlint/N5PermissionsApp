import React from "react";
import { Typography, Grid, Paper, Box } from "@mui/material";

const Dashboard: React.FC = () => {
  return (
    <Box>
      <Typography variant='h4' gutterBottom>
        Dashboard
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6} lg={3}>
          <Paper
            sx={{
              p: 2,
              display: "flex",
              flexDirection: "column",
              height: 140,
            }}
          >
            <Typography
              component='h2'
              variant='h6'
              color='primary'
              gutterBottom
            >
              Permisions
            </Typography>
            <Typography component='p' variant='h4'></Typography>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};

export default Dashboard;
