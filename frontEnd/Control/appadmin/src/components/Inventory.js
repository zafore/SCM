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
  Inventory as InventoryIcon,
  Warning as WarningIcon,
  CheckCircle as CheckCircleIcon,
  TrendingUp as TrendingUpIcon,
  TrendingDown as TrendingDownIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Inventory = () => {
  const [inventory, setInventory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  useEffect(() => {
    fetchInventory();
  }, []);

  const fetchInventory = async () => {
    try {
      setLoading(true);
      // Mock data since we don't have inventory endpoint yet
      const mockInventory = [
        {
          id: 1,
          itemName: 'Office Chair',
          sku: 'CHAIR-001',
          category: 'Furniture',
          currentStock: 25,
          minStock: 10,
          maxStock: 100,
          unitPrice: 150.00,
          status: 'In Stock',
          lastUpdated: '2024-01-15'
        },
        {
          id: 2,
          itemName: 'Laptop Computer',
          sku: 'LAPTOP-002',
          category: 'Electronics',
          currentStock: 5,
          minStock: 10,
          maxStock: 50,
          unitPrice: 1200.00,
          status: 'Low Stock',
          lastUpdated: '2024-01-14'
        }
      ];
      setInventory(mockInventory);
    } catch (error) {
      console.error('Error fetching inventory:', error);
      showSnackbar('Error fetching inventory', 'error');
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

  const getStockStatus = (current, min) => {
    if (current <= min) return { status: 'Low Stock', color: 'error' };
    if (current <= min * 1.5) return { status: 'Medium Stock', color: 'warning' };
    return { status: 'In Stock', color: 'success' };
  };

  const columns = [
    {
      field: 'itemName',
      headerName: 'Item Name',
      width: 200,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <InventoryIcon />
          </Avatar>
          <Box>
            <Typography variant="subtitle2">{params.value}</Typography>
            <Typography variant="caption" color="textSecondary">
              {params.row.sku}
            </Typography>
          </Box>
        </Box>
      )
    },
    {
      field: 'category',
      headerName: 'Category',
      width: 120
    },
    {
      field: 'currentStock',
      headerName: 'Current Stock',
      width: 120,
      renderCell: (params) => {
        const stockStatus = getStockStatus(params.value, params.row.minStock);
        return (
          <Box display="flex" alignItems="center">
            <Typography variant="body2" sx={{ mr: 1 }}>
              {params.value}
            </Typography>
            {stockStatus.status === 'Low Stock' && <WarningIcon color="error" fontSize="small" />}
            {stockStatus.status === 'In Stock' && <CheckCircleIcon color="success" fontSize="small" />}
          </Box>
        );
      }
    },
    {
      field: 'minStock',
      headerName: 'Min Stock',
      width: 100
    },
    {
      field: 'maxStock',
      headerName: 'Max Stock',
      width: 100
    },
    {
      field: 'unitPrice',
      headerName: 'Unit Price',
      width: 120,
      renderCell: (params) => `$${params.value.toFixed(2)}`
    },
    {
      field: 'status',
      headerName: 'Status',
      width: 120,
      renderCell: (params) => {
        const stockStatus = getStockStatus(params.row.currentStock, params.row.minStock);
        return (
          <Chip
            label={stockStatus.status}
            color={stockStatus.color}
            size="small"
          />
        );
      }
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
        <Typography variant="h4">Inventory Management</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => showSnackbar('Add inventory feature coming soon')}
        >
          Add Item
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
                    Total Items
                  </Typography>
                  <Typography variant="h4">{inventory.length}</Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'primary.main' }}>
                  <InventoryIcon />
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
                    Low Stock Items
                  </Typography>
                  <Typography variant="h4" color="error.main">
                    {inventory.filter(item => item.currentStock <= item.minStock).length}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'error.main' }}>
                  <WarningIcon />
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
                    Total Value
                  </Typography>
                  <Typography variant="h4">
                    ${inventory.reduce((sum, item) => sum + (item.currentStock * item.unitPrice), 0).toLocaleString()}
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
                    Categories
                  </Typography>
                  <Typography variant="h4">
                    {new Set(inventory.map(item => item.category)).size}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'info.main' }}>
                  <InventoryIcon />
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
              placeholder="Search inventory..."
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
        </Grid>
      </Paper>

      {/* Inventory Table */}
      <Paper sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={inventory}
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

export default Inventory;
