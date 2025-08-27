import React, { useState, useEffect } from 'react';
import {
  Box,
  Paper,
  Typography,
  Button,
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Grid,
  Card,
  CardContent,
  CardActions,
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
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Tooltip,
  Stepper,
  Step,
  StepLabel,
  StepContent,
  Accordion,
  AccordionSummary,
  AccordionDetails
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Search as SearchIcon,
  ShoppingCart as ShoppingCartIcon,
  Business as BusinessIcon,
  Person as PersonIcon,
  AttachMoney as MoneyIcon,
  CalendarToday as CalendarIcon,
  ExpandMore as ExpandMoreIcon,
  Visibility as ViewIcon,
  Print as PrintIcon,
  Email as EmailIcon,
  CheckCircle as CheckCircleIcon,
  Pending as PendingIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingOrder, setEditingOrder] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  const [formData, setFormData] = useState({
    orderNumber: '',
    supplierId: '',
    orderDate: new Date(),
    expectedDeliveryDate: new Date(),
    totalAmount: 0,
    status: 'Pending',
    priority: 'Medium',
    notes: '',
    items: []
  });

  const orderStatuses = [
    { value: 'Pending', label: 'Pending', color: 'warning' },
    { value: 'Confirmed', label: 'Confirmed', color: 'info' },
    { value: 'Processing', label: 'Processing', color: 'primary' },
    { value: 'Shipped', label: 'Shipped', color: 'secondary' },
    { value: 'Delivered', label: 'Delivered', color: 'success' },
    { value: 'Cancelled', label: 'Cancelled', color: 'error' }
  ];

  const orderSteps = [
    'Order Created',
    'Supplier Confirmed',
    'Processing',
    'Shipped',
    'Delivered'
  ];

  useEffect(() => {
    fetchOrders();
    fetchSuppliers();
  }, []);

  const fetchOrders = async () => {
    try {
      setLoading(true);
      const response = await api.get(ENDPOINTS.ORDERS);
      setOrders(response.data || []);
    } catch (error) {
      console.error('Error fetching orders:', error);
      showSnackbar('Error fetching orders', 'error');
    } finally {
      setLoading(false);
    }
  };

  const fetchSuppliers = async () => {
    try {
      const response = await api.get(ENDPOINTS.SUPPLIERS);
      setSuppliers(response.data || []);
    } catch (error) {
      console.error('Error fetching suppliers:', error);
    }
  };

  const showSnackbar = (message, severity = 'success') => {
    setSnackbar({ open: true, message, severity });
  };

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  const handleOpenDialog = (order = null) => {
    if (order) {
      setEditingOrder(order);
      setFormData({
        orderNumber: order.orderNumber || '',
        supplierId: order.supplierId || '',
        orderDate: new Date(order.orderDate) || new Date(),
        expectedDeliveryDate: new Date(order.expectedDeliveryDate) || new Date(),
        totalAmount: order.totalAmount || 0,
        status: order.status || 'Pending',
        priority: order.priority || 'Medium',
        notes: order.notes || '',
        items: order.items || []
      });
    } else {
      setEditingOrder(null);
      setFormData({
        orderNumber: `ORD-${Date.now()}`,
        supplierId: '',
        orderDate: new Date(),
        expectedDeliveryDate: new Date(),
        totalAmount: 0,
        status: 'Pending',
        priority: 'Medium',
        notes: '',
        items: []
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingOrder(null);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async () => {
    try {
      if (editingOrder) {
        await api.put(`${ENDPOINTS.ORDERS}/${editingOrder.id}`, formData);
        showSnackbar('Order updated successfully');
      } else {
        await api.post(ENDPOINTS.ORDERS, formData);
        showSnackbar('Order created successfully');
      }
      handleCloseDialog();
      fetchOrders();
    } catch (error) {
      console.error('Error saving order:', error);
      showSnackbar('Error saving order', 'error');
    }
  };

  const handleDelete = async (orderId) => {
    if (window.confirm('Are you sure you want to delete this order?')) {
      try {
        await api.delete(`${ENDPOINTS.ORDERS}/${orderId}`);
        showSnackbar('Order deleted successfully');
        fetchOrders();
      } catch (error) {
        console.error('Error deleting order:', error);
        showSnackbar('Error deleting order', 'error');
      }
    }
  };

  const handleStatusChange = async (orderId, newStatus) => {
    try {
      await api.put(`${ENDPOINTS.ORDERS}/${orderId}`, { status: newStatus });
      showSnackbar('Order status updated successfully');
      fetchOrders();
    } catch (error) {
      console.error('Error updating order status:', error);
      showSnackbar('Error updating order status', 'error');
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case 'Delivered':
        return <CheckCircleIcon color="success" />;
      case 'Pending':
        return <PendingIcon color="warning" />;
      case 'Cancelled':
        return <CancelIcon color="error" />;
      default:
        return <PendingIcon color="info" />;
    }
  };

  const getCurrentStep = (status) => {
    switch (status) {
      case 'Pending':
        return 0;
      case 'Confirmed':
        return 1;
      case 'Processing':
        return 2;
      case 'Shipped':
        return 3;
      case 'Delivered':
        return 4;
      default:
        return 0;
    }
  };

  const filteredOrders = orders.filter(order => {
    const matchesSearch = order.orderNumber?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         order.supplierName?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || order.status === filterStatus;
    return matchesSearch && matchesStatus;
  });

  const columns = [
    {
      field: 'orderNumber',
      headerName: 'Order #',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <ShoppingCartIcon />
          </Avatar>
          <Typography variant="subtitle2" fontWeight="bold">
            {params.value}
          </Typography>
        </Box>
      )
    },
    {
      field: 'supplierName',
      headerName: 'Supplier',
      width: 200,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <BusinessIcon sx={{ mr: 1, fontSize: 16 }} />
          {params.value}
        </Box>
      )
    },
    {
      field: 'orderDate',
      headerName: 'Order Date',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <CalendarIcon sx={{ mr: 1, fontSize: 16 }} />
          {new Date(params.value).toLocaleDateString()}
        </Box>
      )
    },
    {
      field: 'totalAmount',
      headerName: 'Total Amount',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <MoneyIcon sx={{ mr: 1, fontSize: 16 }} />
          ${params.value?.toLocaleString()}
        </Box>
      )
    },
    {
      field: 'status',
      headerName: 'Status',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          {getStatusIcon(params.value)}
          <Chip
            label={params.value}
            color={orderStatuses.find(s => s.value === params.value)?.color || 'default'}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>
      )
    },
    {
      field: 'priority',
      headerName: 'Priority',
      width: 120,
      renderCell: (params) => (
        <Chip
          label={params.value}
          color={params.value === 'High' ? 'error' : params.value === 'Medium' ? 'warning' : 'success'}
          size="small"
        />
      )
    },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 150,
      sortable: false,
      renderCell: (params) => (
        <Box>
          <Tooltip title="View Details">
            <IconButton
              size="small"
              onClick={() => setSelectedOrder(params.row)}
            >
              <ViewIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Edit">
            <IconButton
              size="small"
              onClick={() => handleOpenDialog(params.row)}
            >
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Delete">
            <IconButton
              size="small"
              onClick={() => handleDelete(params.row.id)}
              color="error"
            >
              <DeleteIcon />
            </IconButton>
          </Tooltip>
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
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Box>
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
          <Typography variant="h4">Orders Management</Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenDialog()}
          >
            Create Order
          </Button>
        </Box>

        {/* Filters */}
        <Paper sx={{ p: 2, mb: 3 }}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                placeholder="Search orders..."
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
                  {orderStatuses.map(status => (
                    <MenuItem key={status.value} value={status.value}>
                      {status.label}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </Paper>

        {/* Orders Table */}
        <Paper sx={{ height: 600, width: '100%' }}>
          <DataGrid
            rows={orders}
            columns={columns}
            pageSize={10}
            rowsPerPageOptions={[5, 10, 25]}
            checkboxSelection
            disableSelectionOnClick
            loading={loading}
          />
        </Paper>

        {/* Order Details Dialog */}
        {selectedOrder && (
          <Dialog open={!!selectedOrder} onClose={() => setSelectedOrder(null)} maxWidth="md" fullWidth>
            <DialogTitle>
              Order Details - {selectedOrder.orderNumber}
            </DialogTitle>
            <DialogContent>
              <Grid container spacing={3} sx={{ mt: 1 }}>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>Order Information</Typography>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Order Number</Typography>
                    <Typography variant="body1">{selectedOrder.orderNumber}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Supplier</Typography>
                    <Typography variant="body1">{selectedOrder.supplierName}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Order Date</Typography>
                    <Typography variant="body1">
                      {new Date(selectedOrder.orderDate).toLocaleDateString()}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Total Amount</Typography>
                    <Typography variant="body1" fontWeight="bold">
                      ${selectedOrder.totalAmount?.toLocaleString()}
                    </Typography>
                  </Box>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>Order Progress</Typography>
                  <Stepper activeStep={getCurrentStep(selectedOrder.status)} orientation="vertical">
                    {orderSteps.map((label, index) => (
                      <Step key={label}>
                        <StepLabel>{label}</StepLabel>
                      </Step>
                    ))}
                  </Stepper>
                </Grid>
                <Grid item xs={12}>
                  <Typography variant="h6" gutterBottom>Status Management</Typography>
                  <Box display="flex" gap={1} flexWrap="wrap">
                    {orderStatuses.map(status => (
                      <Button
                        key={status.value}
                        variant={selectedOrder.status === status.value ? "contained" : "outlined"}
                        size="small"
                        onClick={() => handleStatusChange(selectedOrder.id, status.value)}
                        disabled={selectedOrder.status === status.value}
                      >
                        {status.label}
                      </Button>
                    ))}
                  </Box>
                </Grid>
              </Grid>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setSelectedOrder(null)}>Close</Button>
              <Button startIcon={<PrintIcon />} variant="outlined">Print</Button>
              <Button startIcon={<EmailIcon />} variant="outlined">Email</Button>
            </DialogActions>
          </Dialog>
        )}

        {/* Add/Edit Dialog */}
        <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
          <DialogTitle>
            {editingOrder ? 'Edit Order' : 'Create New Order'}
          </DialogTitle>
          <DialogContent>
            <Grid container spacing={2} sx={{ mt: 1 }}>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  label="Order Number"
                  name="orderNumber"
                  value={formData.orderNumber}
                  onChange={handleInputChange}
                  required
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <FormControl fullWidth required>
                  <InputLabel>Supplier</InputLabel>
                  <Select
                    name="supplierId"
                    value={formData.supplierId}
                    label="Supplier"
                    onChange={handleInputChange}
                  >
                    {suppliers.map(supplier => (
                      <MenuItem key={supplier.id} value={supplier.id}>
                        {supplier.supplierName}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12} md={6}>
                <DatePicker
                  label="Order Date"
                  value={formData.orderDate}
                  onChange={(date) => setFormData(prev => ({ ...prev, orderDate: date }))}
                  renderInput={(params) => <TextField {...params} fullWidth />}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <DatePicker
                  label="Expected Delivery Date"
                  value={formData.expectedDeliveryDate}
                  onChange={(date) => setFormData(prev => ({ ...prev, expectedDeliveryDate: date }))}
                  renderInput={(params) => <TextField {...params} fullWidth />}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <TextField
                  fullWidth
                  label="Total Amount"
                  name="totalAmount"
                  type="number"
                  value={formData.totalAmount}
                  onChange={handleInputChange}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <FormControl fullWidth>
                  <InputLabel>Status</InputLabel>
                  <Select
                    name="status"
                    value={formData.status}
                    label="Status"
                    onChange={handleInputChange}
                  >
                    {orderStatuses.map(status => (
                      <MenuItem key={status.value} value={status.value}>
                        {status.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12} md={4}>
                <FormControl fullWidth>
                  <InputLabel>Priority</InputLabel>
                  <Select
                    name="priority"
                    value={formData.priority}
                    label="Priority"
                    onChange={handleInputChange}
                  >
                    <MenuItem value="Low">Low</MenuItem>
                    <MenuItem value="Medium">Medium</MenuItem>
                    <MenuItem value="High">High</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Notes"
                  name="notes"
                  value={formData.notes}
                  onChange={handleInputChange}
                  multiline
                  rows={3}
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCloseDialog}>Cancel</Button>
            <Button onClick={handleSubmit} variant="contained">
              {editingOrder ? 'Update' : 'Create'}
            </Button>
          </DialogActions>
        </Dialog>

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
    </LocalizationProvider>
  );
};

export default Orders;
