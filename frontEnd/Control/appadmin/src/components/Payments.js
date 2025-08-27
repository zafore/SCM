import React, { useState, useEffect } from 'react';
import {
  Box,
  Paper,
  Typography,
  Button,
  TextField,
  Grid,
  Card,
  CardContent,
  IconButton,
  Chip,
  Avatar,
  Alert,
  Snackbar,
  CircularProgress,
  InputAdornment,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Tooltip
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Search as SearchIcon,
  Payment as PaymentIcon,
  AttachMoney as MoneyIcon,
  CheckCircle as CheckCircleIcon,
  Pending as PendingIcon,
  Cancel as CancelIcon,
  TrendingUp as TrendingUpIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Payments = () => {
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    fetchPayments();
  }, []);

  const fetchPayments = async () => {
    try {
      setLoading(true);
      // Mock data since we don't have payments endpoint yet
      const mockPayments = [
        {
          id: 1,
          paymentNumber: 'PAY-2024-001',
          orderNumber: 'ORD-2024-001',
          supplierName: 'ABC Supplies Ltd',
          amount: 5000.00,
          paymentDate: '2024-01-15',
          dueDate: '2024-01-20',
          status: 'Completed',
          paymentMethod: 'Bank Transfer',
          reference: 'TXN-123456'
        },
        {
          id: 2,
          paymentNumber: 'PAY-2024-002',
          orderNumber: 'ORD-2024-002',
          supplierName: 'Tech Solutions Inc',
          amount: 12000.00,
          paymentDate: '2024-01-16',
          dueDate: '2024-01-25',
          status: 'Pending',
          paymentMethod: 'Check',
          reference: 'CHK-789012'
        }
      ];
      setPayments(mockPayments);
    } catch (error) {
      console.error('Error fetching payments:', error);
      showSnackbar('Error fetching payments', 'error');
    } finally {
      setLoading(false);
    }
  };

  const showSnackbar = (message, severity = 'success') => {
    setSnackbar({ open: true, message, severity });
  };

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case 'Completed':
        return <CheckCircleIcon color="success" />;
      case 'Pending':
        return <PendingIcon color="warning" />;
      case 'Failed':
        return <CancelIcon color="error" />;
      default:
        return <PaymentIcon color="info" />;
    }
  };

  const columns = [
    {
      field: 'paymentNumber',
      headerName: 'Payment #',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <PaymentIcon />
          </Avatar>
          <Typography variant="subtitle2" fontWeight="bold">
            {params.value}
          </Typography>
        </Box>
      )
    },
    {
      field: 'orderNumber',
      headerName: 'Order #',
      width: 120
    },
    {
      field: 'supplierName',
      headerName: 'Supplier',
      width: 200
    },
    {
      field: 'amount',
      headerName: 'Amount',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <MoneyIcon sx={{ mr: 1, fontSize: 16 }} />
          ${params.value.toLocaleString()}
        </Box>
      )
    },
    {
      field: 'paymentDate',
      headerName: 'Payment Date',
      width: 120
    },
    {
      field: 'dueDate',
      headerName: 'Due Date',
      width: 120
    },
    {
      field: 'paymentMethod',
      headerName: 'Method',
      width: 120
    },
    {
      field: 'status',
      headerName: 'Status',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          {getStatusIcon(params.value)}
          <Chip
            label={params.value}
            color={params.value === 'Completed' ? 'success' : params.value === 'Pending' ? 'warning' : 'error'}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>
      )
    }
  ];

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4">Payments Management</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => showSnackbar('Add payment feature coming soon')}
        >
          Process Payment
        </Button>
      </Box>

      {/* Summary Cards */}
      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="textSecondary" gutterBottom variant="h6">
                    Total Payments
                  </Typography>
                  <Typography variant="h4">{payments.length}</Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'primary.main' }}>
                  <PaymentIcon />
                </Avatar>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="textSecondary" gutterBottom variant="h6">
                    Completed
                  </Typography>
                  <Typography variant="h4" color="success.main">
                    {payments.filter(p => p.status === 'Completed').length}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'success.main' }}>
                  <CheckCircleIcon />
                </Avatar>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="textSecondary" gutterBottom variant="h6">
                    Pending
                  </Typography>
                  <Typography variant="h4" color="warning.main">
                    {payments.filter(p => p.status === 'Pending').length}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'warning.main' }}>
                  <PendingIcon />
                </Avatar>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between">
                <Box>
                  <Typography color="textSecondary" gutterBottom variant="h6">
                    Total Amount
                  </Typography>
                  <Typography variant="h4">
                    ${payments.reduce((sum, p) => sum + p.amount, 0).toLocaleString()}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'info.main' }}>
                  <TrendingUpIcon />
                </Avatar>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      {/* Filters */}
      <Paper sx={{ p: 2, mb: 3 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              placeholder="Search payments..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          <Grid item xs={12} md={3}>
            <FormControl fullWidth>
              <InputLabel>Status</InputLabel>
              <Select
                value={filterStatus}
                label="Status"
                onChange={(e) => setFilterStatus(e.target.value)}
              >
                <MenuItem value="all">All</MenuItem>
                <MenuItem value="Completed">Completed</MenuItem>
                <MenuItem value="Pending">Pending</MenuItem>
                <MenuItem value="Failed">Failed</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {/* Payments Table */}
      <Paper sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={payments}
          columns={columns}
          pageSize={10}
          rowsPerPageOptions={[5, 10, 25]}
          checkboxSelection
          disableSelectionOnClick
          loading={loading}
        />
      </Paper>

      {/* Snackbar */}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
      >
        <Alert onClose={handleCloseSnackbar} severity={snackbar.severity}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default Payments;
