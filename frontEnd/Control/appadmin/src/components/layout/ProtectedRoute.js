import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { Box, CircularProgress, Typography } from '@mui/material';
import AuthService from '../../services/AuthServices';
import { ROLES } from '../../config';

const ProtectedRoute = ({ children, allowedRoles = [] }) => {
  const location = useLocation();
  const user = AuthService.getCurrentUser();
  const isLoggedIn = AuthService.isLoggedIn();

  // Show loading while checking authentication
  if (!user && isLoggedIn === null) {
    return (
      <Box
        display="flex"
        flexDirection="column"
        justifyContent="center"
        alignItems="center"
        minHeight="100vh"
      >
        <CircularProgress size={60} />
        <Typography variant="h6" sx={{ mt: 2 }}>
          Loading...
        </Typography>
      </Box>
    );
  }

  // Redirect to login if not authenticated
  if (!isLoggedIn || !user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Check role-based access
  if (allowedRoles.length > 0 && !allowedRoles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
};

export default ProtectedRoute;
