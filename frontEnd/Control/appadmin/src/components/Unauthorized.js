import React from 'react';
import { Container, Paper, Typography, Button, Box } from '@mui/material';
import { Security as SecurityIcon, ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';

const Unauthorized = () => {
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate(-1);
  };

  const handleGoHome = () => {
    navigate('/dashboard');
  };

  return (
    <Container component="main" maxWidth="sm">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Paper
          elevation={3}
          sx={{
            padding: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            width: '100%',
          }}
        >
          <SecurityIcon sx={{ fontSize: 80, color: 'error.main', mb: 2 }} />
          
          <Typography component="h1" variant="h4" sx={{ mb: 2, color: 'error.main' }}>
            Access Denied
          </Typography>
          
          <Typography variant="h6" sx={{ mb: 3, textAlign: 'center', color: 'text.secondary' }}>
            You don't have permission to access this page.
          </Typography>
          
          <Typography variant="body1" sx={{ mb: 4, textAlign: 'center' }}>
            Please contact your administrator if you believe this is an error.
          </Typography>
          
          <Box sx={{ display: 'flex', gap: 2 }}>
            <Button
              variant="outlined"
              startIcon={<ArrowBackIcon />}
              onClick={handleGoBack}
            >
              Go Back
            </Button>
            <Button
              variant="contained"
              onClick={handleGoHome}
            >
              Go to Dashboard
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default Unauthorized;