import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
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
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Alert,
  Snackbar,
  CircularProgress,
  InputAdornment,
  FormControl,
  InputLabel,
  Select,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Tooltip
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  MoreVert as MoreVertIcon,
  Search as SearchIcon,
  Business as BusinessIcon,
  Email as EmailIcon,
  Phone as PhoneIcon,
  LocationOn as LocationIcon,
  Star as StarIcon,
  StarBorder as StarBorderIcon,
  Visibility as ViewIcon,
  Assignment as ContractIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Suppliers = () => {
  const { t } = useTranslation();
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingSupplier, setEditingSupplier] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });
  const [anchorEl, setAnchorEl] = useState(null);
  const [selectedSupplier, setSelectedSupplier] = useState(null);

  const [formData, setFormData] = useState({
    supplierName: '',
    email: '',
    phone: '',
    address: '',
    city: '',
    country: '',
    contactPerson: '',
    website: '',
    status: 'Active',
    rating: 0,
    notes: ''
  });

  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      setLoading(true);
      const response = await api.get(ENDPOINTS.SUPPLIERS);
      setSuppliers(response.data || []);
    } catch (error) {
      console.error('Error fetching suppliers:', error);
      showSnackbar('Error fetching suppliers', 'error');
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

  const handleOpenDialog = (supplier = null) => {
    if (supplier) {
      setEditingSupplier(supplier);
      setFormData({
        supplierName: supplier.supplierName || '',
        email: supplier.email || '',
        phone: supplier.phone || '',
        address: supplier.address || '',
        city: supplier.city || '',
        country: supplier.country || '',
        contactPerson: supplier.contactPerson || '',
        website: supplier.website || '',
        status: supplier.status || 'Active',
        rating: supplier.rating || 0,
        notes: supplier.notes || ''
      });
    } else {
      setEditingSupplier(null);
      setFormData({
        supplierName: '',
        email: '',
        phone: '',
        address: '',
        city: '',
        country: '',
        contactPerson: '',
        website: '',
        status: 'Active',
        rating: 0,
        notes: ''
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingSupplier(null);
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
      if (editingSupplier) {
        await api.put(`${ENDPOINTS.SUPPLIERS}/${editingSupplier.id}`, formData);
        showSnackbar('Supplier updated successfully');
      } else {
        await api.post(ENDPOINTS.SUPPLIERS, formData);
        showSnackbar('Supplier created successfully');
      }
      handleCloseDialog();
      fetchSuppliers();
    } catch (error) {
      console.error('Error saving supplier:', error);
      showSnackbar('Error saving supplier', 'error');
    }
  };

  const handleDelete = async (supplierId) => {
    if (window.confirm('Are you sure you want to delete this supplier?')) {
      try {
        await api.delete(`${ENDPOINTS.SUPPLIERS}/${supplierId}`);
        showSnackbar('Supplier deleted successfully');
        fetchSuppliers();
      } catch (error) {
        console.error('Error deleting supplier:', error);
        showSnackbar('Error deleting supplier', 'error');
      }
    }
  };

  const handleMenuOpen = (event, supplier) => {
    setAnchorEl(event.currentTarget);
    setSelectedSupplier(supplier);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedSupplier(null);
  };

  const filteredSuppliers = suppliers.filter(supplier => {
    const matchesSearch = supplier.supplierName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         supplier.email?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         supplier.contactPerson?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || supplier.status === filterStatus;
    return matchesSearch && matchesStatus;
  });

  const paginatedSuppliers = filteredSuppliers.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

  const columns = [
    {
      field: 'supplierName',
      headerName: 'Supplier Name',
      width: 200,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <BusinessIcon />
          </Avatar>
          <Box>
            <Typography variant="subtitle2">{params.value}</Typography>
            <Typography variant="caption" color="textSecondary">
              {params.row.contactPerson}
            </Typography>
          </Box>
        </Box>
      )
    },
    {
      field: 'email',
      headerName: 'Email',
      width: 200,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <EmailIcon sx={{ mr: 1, fontSize: 16 }} />
          {params.value}
        </Box>
      )
    },
    {
      field: 'phone',
      headerName: 'Phone',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <PhoneIcon sx={{ mr: 1, fontSize: 16 }} />
          {params.value}
        </Box>
      )
    },
    {
      field: 'country',
      headerName: 'Country',
      width: 120
    },
    {
      field: 'status',
      headerName: 'Status',
      width: 120,
      renderCell: (params) => (
        <Chip
          label={params.value}
          color={params.value === 'Active' ? 'success' : 'default'}
          size="small"
        />
      )
    },
    {
      field: 'rating',
      headerName: 'Rating',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          {[1, 2, 3, 4, 5].map((star) => (
            <StarIcon
              key={star}
              sx={{
                fontSize: 16,
                color: star <= params.value ? 'gold' : 'grey.300'
              }}
            />
          ))}
        </Box>
      )
    },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 100,
      sortable: false,
      renderCell: (params) => (
        <Box>
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
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4">{t('suppliers.title')}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpenDialog()}
        >
          {t('suppliers.addSupplier')}
        </Button>
      </Box>

      {/* Filters */}
      <Paper sx={{ p: 2, mb: 3 }}>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={6}>
            <TextField
              fullWidth
              placeholder={t('common.search') + '...'}
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
              <InputLabel>{t('common.status')}</InputLabel>
              <Select
                value={filterStatus}
                label={t('common.status')}
                onChange={(e) => setFilterStatus(e.target.value)}
              >
                <MenuItem value="all">{t('common.all')}</MenuItem>
                <MenuItem value="Active">{t('common.active')}</MenuItem>
                <MenuItem value="Inactive">{t('common.inactive')}</MenuItem>
                <MenuItem value="Pending">{t('common.pending')}</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {/* Suppliers Table */}
      <Paper sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={suppliers}
          columns={columns}
          pageSize={10}
          rowsPerPageOptions={[5, 10, 25]}
          checkboxSelection
          disableSelectionOnClick
          loading={loading}
        />
      </Paper>

      {/* Add/Edit Dialog */}
      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>
          {editingSupplier ? t('suppliers.editSupplier') : t('suppliers.addSupplier')}
        </DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Supplier Name"
                name="supplierName"
                value={formData.supplierName}
                onChange={handleInputChange}
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Contact Person"
                name="contactPerson"
                value={formData.contactPerson}
                onChange={handleInputChange}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleInputChange}
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Phone"
                name="phone"
                value={formData.phone}
                onChange={handleInputChange}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Address"
                name="address"
                value={formData.address}
                onChange={handleInputChange}
                multiline
                rows={2}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="City"
                name="city"
                value={formData.city}
                onChange={handleInputChange}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Country"
                name="country"
                value={formData.country}
                onChange={handleInputChange}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Website"
                name="website"
                value={formData.website}
                onChange={handleInputChange}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <FormControl fullWidth>
                <InputLabel>Status</InputLabel>
                <Select
                  name="status"
                  value={formData.status}
                  label="Status"
                  onChange={handleInputChange}
                >
                  <MenuItem value="Active">Active</MenuItem>
                  <MenuItem value="Inactive">Inactive</MenuItem>
                  <MenuItem value="Pending">Pending</MenuItem>
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
            {editingSupplier ? 'Update' : 'Create'}
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
  );
};

export default Suppliers;
