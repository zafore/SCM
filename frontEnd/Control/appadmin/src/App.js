import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import CssBaseline from '@mui/material/CssBaseline';
import AuthService from "./services/AuthServices";
import MainLayout from "./components/layout/MainLayout";
import ProtectedRoute from "./components/layout/ProtectedRoute";
import Login from "./components/Login";
import Unauthorized from "./components/Unauthorized";
import Dashboard from "./components/Dashboard";
import Suppliers from "./components/Suppliers";
import Orders from "./components/Orders";
import Inventory from "./components/Inventory";
import Payments from "./components/Payments";
import Accounting from "./components/Accounting";
import Contracts from "./components/Contracts";
import UserManagement from "./components/UserManagement";
import AuditLogs from "./components/AuditLogs";
import ThemeProvider from "./components/ThemeProvider";
import { ROLES } from "./config";
import './i18n';

function App() {
  const isLoggedIn = AuthService.isLoggedIn();

  return (
    <ThemeProvider>
      <CssBaseline />
      <Router>
        <Routes>
          {/* Public routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/unauthorized" element={<Unauthorized />} />

          {/* Protected routes with layout */}
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER, ROLES.USER]}>
                <MainLayout>
                  <Dashboard />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/suppliers"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <Suppliers />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/orders"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER, ROLES.USER]}>
                <MainLayout>
                  <Orders />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/inventory"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <Inventory />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/payments"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <Payments />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/accounting"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <Accounting />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/contracts"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <Contracts />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/users"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN]}>
                <MainLayout>
                  <UserManagement />
                </MainLayout>
              </ProtectedRoute>
            }
          />
          
          <Route
            path="/audit"
            element={
              <ProtectedRoute allowedRoles={[ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]}>
                <MainLayout>
                  <AuditLogs />
                </MainLayout>
              </ProtectedRoute>
            }
          />

          {/* Default route */}
          <Route 
            path="*" 
            element={<Navigate to={isLoggedIn ? "/dashboard" : "/login"} replace />} 
          />
        </Routes>
      </Router>
    </ThemeProvider>
  );
}

export default App;
