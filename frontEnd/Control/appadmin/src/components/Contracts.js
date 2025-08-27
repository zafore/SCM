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
  Alert,
  Snackbar,
  CircularProgress,
  InputAdornment,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Tooltip,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Search as SearchIcon,
  Assignment as AssignmentIcon,
  Business as BusinessIcon,
  CalendarToday as CalendarIcon,
  AttachMoney as MoneyIcon,
  Visibility as ViewIcon,
  Download as DownloadIcon,
  Print as PrintIcon,
  Email as EmailIcon,
  CheckCircle as CheckCircleIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const Contracts = () => {
  const { t } = useTranslation();
  const [contracts, setContracts] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingContract, setEditingContract] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');
  const [selectedContract, setSelectedContract] = useState(null);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  const [formData, setFormData] = useState({
    contractNumber: '',
    supplierId: '',
    title: '',
    description: '',
    startDate: new Date(),
    endDate: new Date(),
    totalValue: 0,
    status: 'Draft',
    contractType: 'Service',
    terms: '',
    renewalTerms: '',
    notes: ''
  });

  const contractStatuses = [
    { value: 'Draft', label: 'Draft', color: 'default' },
    { value: 'Active', label: 'Active', color: 'success' },
    { value: 'Expired', label: 'Expired', color: 'error' },
    { value: 'Terminated', label: 'Terminated', color: 'warning' },
    { value: 'Under Review', label: 'Under Review', color: 'info' }
  ];

  const contractTypes = [
    'Service',
    'Supply',
    'Maintenance',
    'Consulting',
    'Software License',
    'Equipment Lease'
  ];

  useEffect(() => {
    fetchContracts();
    fetchSuppliers();
  }, []);

  const fetchContracts = async () => {
    try {
      setLoading(true);
      // Since we don't have a contracts endpoint yet, we'll simulate data
      const mockContracts = [
        {
          id: 1,
          contractNumber: 'CNT-2024-001',
          supplierName: 'ABC Supplies Ltd',
          title: 'Office Supplies Contract',
          startDate: '2024-01-01',
          endDate: '2024-12-31',
          totalValue: 50000,
          status: 'Active',
          contractType: 'Supply'
        },
        {
          id: 2,
          contractNumber: 'CNT-2024-002',
          supplierName: 'Tech Solutions Inc',
          title: 'IT Services Agreement',
          startDate: '2024-02-01',
          endDate: '2025-01-31',
          totalValue: 120000,
          status: 'Active',
          contractType: 'Service'
        }
      ];
      setContracts(mockContracts);
    } catch (error) {
      console.error('Error fetching contracts:', error);
      showSnackbar('Error fetching contracts', 'error');
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

  const handleOpenDialog = (contract = null) => {
    if (contract) {
      setEditingContract(contract);
      setFormData({
        contractNumber: contract.contractNumber || '',
        supplierId: contract.supplierId || '',
        title: contract.title || '',
        description: contract.description || '',
        startDate: new Date(contract.startDate) || new Date(),
        endDate: new Date(contract.endDate) || new Date(),
        totalValue: contract.totalValue || 0,
        status: contract.status || 'Draft',
        contractType: contract.contractType || 'Service',
        terms: contract.terms || '',
        renewalTerms: contract.renewalTerms || '',
        notes: contract.notes || ''
      });
    } else {
      setEditingContract(null);
      setFormData({
        contractNumber: `CNT-${Date.now()}`,
        supplierId: '',
        title: '',
        description: '',
        startDate: new Date(),
        endDate: new Date(),
        totalValue: 0,
        status: 'Draft',
        contractType: 'Service',
        terms: '',
        renewalTerms: '',
        notes: ''
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingContract(null);
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
      // Simulate API call
      showSnackbar(editingContract ? 'Contract updated successfully' : 'Contract created successfully');
      handleCloseDialog();
      fetchContracts();
    } catch (error) {
      console.error('Error saving contract:', error);
      showSnackbar('Error saving contract', 'error');
    }
  };

  const handleDelete = async (contractId) => {
    if (window.confirm('Are you sure you want to delete this contract?')) {
      try {
        // Simulate API call
        showSnackbar('Contract deleted successfully');
        fetchContracts();
      } catch (error) {
        console.error('Error deleting contract:', error);
        showSnackbar('Error deleting contract', 'error');
      }
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case 'Active':
        return <CheckCircleIcon color="success" />;
      case 'Expired':
        return <CancelIcon color="error" />;
      case 'Draft':
        return <EditIcon color="default" />;
      default:
        return <AssignmentIcon color="info" />;
    }
  };

  const filteredContracts = contracts.filter(contract => {
    const matchesSearch = contract.contractNumber?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         contract.title?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         contract.supplierName?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = filterStatus === 'all' || contract.status === filterStatus;
    return matchesSearch && matchesStatus;
  });

  const columns = [
    {
      field: 'contractNumber',
      headerName: 'Contract #',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <AssignmentIcon />
          </Avatar>
          <Typography variant="subtitle2" fontWeight="bold">
            {params.value}
          </Typography>
        </Box>
      )
    },
    {
      field: 'title',
      headerName: 'Title',
      width: 250
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
      field: 'contractType',
      headerName: 'Type',
      width: 120
    },
    {
      field: 'startDate',
      headerName: 'Start Date',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <CalendarIcon sx={{ mr: 1, fontSize: 16 }} />
          {new Date(params.value).toLocaleDateString()}
        </Box>
      )
    },
    {
      field: 'endDate',
      headerName: 'End Date',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <CalendarIcon sx={{ mr: 1, fontSize: 16 }} />
          {new Date(params.value).toLocaleDateString()}
        </Box>
      )
    },
    {
      field: 'totalValue',
      headerName: 'Value',
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
            color={contractStatuses.find(s => s.value === params.value)?.color || 'default'}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>
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
              onClick={() => setSelectedContract(params.row)}
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
          <Typography variant="h4">{t('contracts.title')}</Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenDialog()}
          >
            {t('contracts.addContract')}
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
                  <MenuItem value="all">All</MenuItem>
                  {contractStatuses.map(status => (
                    <MenuItem key={status.value} value={status.value}>
                      {status.label}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </Paper>

        {/* Contracts Table */}
        <Paper sx={{ height: 600, width: '100%' }}>
          <DataGrid
            rows={contracts}
            columns={columns}
            pageSize={10}
            rowsPerPageOptions={[5, 10, 25]}
            checkboxSelection
            disableSelectionOnClick
            loading={loading}
          />
        </Paper>

        {/* Contract Details Dialog */}
        {selectedContract && (
          <Dialog open={!!selectedContract} onClose={() => setSelectedContract(null)} maxWidth="md" fullWidth>
            <DialogTitle>
              Contract Details - {selectedContract.contractNumber}
            </DialogTitle>
            <DialogContent>
              <Grid container spacing={3} sx={{ mt: 1 }}>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>Contract Information</Typography>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Contract Number</Typography>
                    <Typography variant="body1">{selectedContract.contractNumber}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Title</Typography>
                    <Typography variant="body1">{selectedContract.title}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Supplier</Typography>
                    <Typography variant="body1">{selectedContract.supplierName}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Type</Typography>
                    <Typography variant="body1">{selectedContract.contractType}</Typography>
                  </Box>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>Contract Terms</Typography>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Start Date</Typography>
                    <Typography variant="body1">
                      {new Date(selectedContract.startDate).toLocaleDateString()}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">End Date</Typography>
                    <Typography variant="body1">
                      {new Date(selectedContract.endDate).toLocaleDateString()}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Total Value</Typography>
                    <Typography variant="body1" fontWeight="bold">
                      ${selectedContract.totalValue?.toLocaleString()}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Status</Typography>
                    <Chip
                      label={selectedContract.status}
                      color={contractStatuses.find(s => s.value === selectedContract.status)?.color || 'default'}
                    />
                  </Box>
                </Grid>
              </Grid>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setSelectedContract(null)}>Close</Button>
              <Button startIcon={<DownloadIcon />} variant="outlined">Download</Button>
              <Button startIcon={<PrintIcon />} variant="outlined">Print</Button>
              <Button startIcon={<EmailIcon />} variant="outlined">Email</Button>
            </DialogActions>
          </Dialog>
        )}

        {/* Add/Edit Dialog */}
        <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
          <DialogTitle>
            {editingContract ? 'Edit Contract' : 'Create New Contract'}
          </DialogTitle>
          <DialogContent>
            <Grid container spacing={2} sx={{ mt: 1 }}>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  label="Contract Number"
                  name="contractNumber"
                  value={formData.contractNumber}
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
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Contract Title"
                  name="title"
                  value={formData.title}
                  onChange={handleInputChange}
                  required
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Description"
                  name="description"
                  value={formData.description}
                  onChange={handleInputChange}
                  multiline
                  rows={3}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <DatePicker
                  label="Start Date"
                  value={formData.startDate}
                  onChange={(date) => setFormData(prev => ({ ...prev, startDate: date }))}
                  renderInput={(params) => <TextField {...params} fullWidth />}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <DatePicker
                  label="End Date"
                  value={formData.endDate}
                  onChange={(date) => setFormData(prev => ({ ...prev, endDate: date }))}
                  renderInput={(params) => <TextField {...params} fullWidth />}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <TextField
                  fullWidth
                  label="Total Value"
                  name="totalValue"
                  type="number"
                  value={formData.totalValue}
                  onChange={handleInputChange}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <FormControl fullWidth>
                  <InputLabel>Contract Type</InputLabel>
                  <Select
                    name="contractType"
                    value={formData.contractType}
                    label="Contract Type"
                    onChange={handleInputChange}
                  >
                    {contractTypes.map(type => (
                      <MenuItem key={type} value={type}>
                        {type}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
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
                    {contractStatuses.map(status => (
                      <MenuItem key={status.value} value={status.value}>
                        {status.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Terms & Conditions"
                  name="terms"
                  value={formData.terms}
                  onChange={handleInputChange}
                  multiline
                  rows={4}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Renewal Terms"
                  name="renewalTerms"
                  value={formData.renewalTerms}
                  onChange={handleInputChange}
                  multiline
                  rows={2}
                />
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
              {editingContract ? 'Update' : 'Create'}
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

export default Contracts;
