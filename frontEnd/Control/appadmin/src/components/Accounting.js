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
  Assessment as AssessmentIcon,
  AttachMoney as MoneyIcon,
  TrendingUp as TrendingUpIcon,
  TrendingDown as TrendingDownIcon,
  AccountBalance as AccountBalanceIcon,
  Receipt as ReceiptIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Accounting = () => {
  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterType, setFilterType] = useState('all');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    fetchTransactions();
  }, []);

  const fetchTransactions = async () => {
    try {
      setLoading(true);
      // Mock data since we don't have accounting endpoint yet
      const mockTransactions = [
        {
          id: 1,
          transactionNumber: 'TXN-2024-001',
          date: '2024-01-15',
          description: 'Payment to ABC Supplies Ltd',
          type: 'Expense',
          amount: 5000.00,
          account: 'Accounts Payable',
          reference: 'PAY-2024-001',
          status: 'Posted'
        },
        {
          id: 2,
          transactionNumber: 'TXN-2024-002',
          date: '2024-01-16',
          description: 'Revenue from Product Sales',
          type: 'Income',
          amount: 15000.00,
          account: 'Accounts Receivable',
          reference: 'SALE-2024-001',
          status: 'Posted'
        }
      ];
      setTransactions(mockTransactions);
    } catch (error) {
      console.error('Error fetching transactions:', error);
      showSnackbar('Error fetching transactions', 'error');
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

  const columns = [
    {
      field: 'transactionNumber',
      headerName: 'Transaction #',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <ReceiptIcon />
          </Avatar>
          <Typography variant="subtitle2" fontWeight="bold">
            {params.value}
          </Typography>
        </Box>
      )
    },
    {
      field: 'date',
      headerName: 'Date',
      width: 120
    },
    {
      field: 'description',
      headerName: 'Description',
      width: 250
    },
    {
      field: 'type',
      headerName: 'Type',
      width: 100,
      renderCell: (params) => (
        <Chip
          label={params.value}
          color={params.value === 'Income' ? 'success' : 'error'}
          size="small"
        />
      )
    },
    {
      field: 'amount',
      headerName: 'Amount',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <MoneyIcon sx={{ mr: 1, fontSize: 16 }} />
          <Typography color={params.row.type === 'Income' ? 'success.main' : 'error.main'}>
            {params.row.type === 'Income' ? '+' : '-'}${params.value.toLocaleString()}
          </Typography>
        </Box>
      )
    },
    {
      field: 'account',
      headerName: 'Account',
      width: 150
    },
    {
      field: 'status',
      headerName: 'Status',
      width: 100,
      renderCell: (params) => (
        <Chip
          label={params.value}
          color="success"
          size="small"
        />
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

  const totalIncome = transactions.filter(t => t.type === 'Income').reduce((sum, t) => sum + t.amount, 0);
  const totalExpenses = transactions.filter(t => t.type === 'Expense').reduce((sum, t) => sum + t.amount, 0);
  const netIncome = totalIncome - totalExpenses;

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4">Accounting & Finance</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => showSnackbar('Add transaction feature coming soon')}
        >
          Add Transaction
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
                    Total Income
                  </Typography>
                  <Typography variant="h4" color="success.main">
                    ${totalIncome.toLocaleString()}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'success.main' }}>
                  <TrendingUpIcon />
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
                    Total Expenses
                  </Typography>
                  <Typography variant="h4" color="error.main">
                    ${totalExpenses.toLocaleString()}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'error.main' }}>
                  <TrendingDownIcon />
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
                    Net Income
                  </Typography>
                  <Typography variant="h4" color={netIncome >= 0 ? 'success.main' : 'error.main'}>
                    ${netIncome.toLocaleString()}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: netIncome >= 0 ? 'success.main' : 'error.main' }}>
                  <AccountBalanceIcon />
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
                    Total Transactions
                  </Typography>
                  <Typography variant="h4">{transactions.length}</Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'primary.main' }}>
                  <AssessmentIcon />
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
              placeholder="Search transactions..."
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
              <InputLabel>Type</InputLabel>
              <Select
                value={filterType}
                label="Type"
                onChange={(e) => setFilterType(e.target.value)}
              >
                <MenuItem value="all">All</MenuItem>
                <MenuItem value="Income">Income</MenuItem>
                <MenuItem value="Expense">Expense</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {/* Transactions Table */}
      <Paper sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={transactions}
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

export default Accounting;
