// src/App.tsx
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ThemeProvider, createTheme, CssBaseline } from "@mui/material";
import Layout from "./components/Layout";
import Dashboard from "./pages/Dashboard/Dashboard";
import PermissionList from "./pages/PermissionList/PermissionList";
import PermissionForm from "./components/PermissionForm";
import PermissionTypesManagement from "./components/PermissionTypesManagement";

const theme = createTheme({
  palette: {
    primary: {
      main: "#1976d2",
    },
    secondary: {
      main: "#dc004e",
    },
  },
});

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Layout>
          <Routes>
            <Route path='/' element={<Dashboard />} />
            <Route path='/permissions' element={<PermissionList />} />
            <Route path='/permissions/new' element={<PermissionForm />} />
            <Route path='/permissions/edit/:id' element={<PermissionForm />} />
            <Route
              path='/permission-types'
              element={<PermissionTypesManagement />}
            />
          </Routes>
        </Layout>
      </Router>
    </ThemeProvider>
  );
};

export default App;
