import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
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
  Tooltip,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions
} from '@mui/material';
import {
  Search as SearchIcon,
  Security as SecurityIcon,
  Person as PersonIcon,
  Business as BusinessIcon,
  ShoppingCart as ShoppingCartIcon,
  Assessment as AssessmentIcon,
  ExpandMore as ExpandMoreIcon,
  Visibility as ViewIcon,
  FilterList as FilterIcon,
  Download as DownloadIcon,
  Refresh as RefreshIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import api from '../services/api';
import { ENDPOINTS } from '../config';

const AuditLogs = () => {
  const { t } = useTranslation();
  const [auditLogs, setAuditLogs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterAction, setFilterAction] = useState('all');
  const [filterEntity, setFilterEntity] = useState('all');
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [selectedLog, setSelectedLog] = useState(null);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  const actions = [
    'LOGIN', 'LOGOUT', 'CREATE', 'UPDATE', 'DELETE', 'VIEW'
  ];

  const entities = [
    'User', 'Supplier', 'Order', 'Payment', 'Inventory', 'Contract'
  ];

  useEffect(() => {
    fetchAuditLogs();
  }, []);

  const fetchAuditLogs = async () => {
    try {
      setLoading(true);
      const response = await api.get(ENDPOINTS.AUDIT + '/recent');
      setAuditLogs(response.data?.data || []);
    } catch (error) {
      console.error('Error fetching audit logs:', error);
      showSnackbar('Error fetching audit logs', 'error');
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

  const getActionIcon = (action) => {
    switch (action) {
      case 'LOGIN':
        return <PersonIcon color="success" />;
      case 'LOGOUT':
        return <PersonIcon color="warning" />;
      case 'CREATE':
        return <BusinessIcon color="info" />;
      case 'UPDATE':
        return <ShoppingCartIcon color="primary" />;
      case 'DELETE':
        return <AssessmentIcon color="error" />;
      case 'VIEW':
        return <ViewIcon color="default" />;
      default:
        return <SecurityIcon color="default" />;
    }
  };

  const getActionColor = (action) => {
    switch (action) {
      case 'LOGIN':
        return 'success';
      case 'LOGOUT':
        return 'warning';
      case 'CREATE':
        return 'info';
      case 'UPDATE':
        return 'primary';
      case 'DELETE':
        return 'error';
      case 'VIEW':
        return 'default';
      default:
        return 'default';
    }
  };

  const columns = [
    {
      field: 'timestamp',
      headerName: 'Date & Time',
      width: 180,
      renderCell: (params) => (
        <Typography variant="body2">
          {new Date(params.value).toLocaleString()}
        </Typography>
      )
    },
    {
      field: 'action',
      headerName: 'Action',
      width: 120,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          {getActionIcon(params.value)}
          <Chip
            label={params.value}
            color={getActionColor(params.value)}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>
      )
    },
    {
      field: 'entityType',
      headerName: 'Entity',
      width: 120
    },
    {
      field: 'entityName',
      headerName: 'Entity Name',
      width: 200
    },
    {
      field: 'userName',
      headerName: 'User',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 1, width: 24, height: 24, bgcolor: 'primary.main' }}>
            <PersonIcon fontSize="small" />
          </Avatar>
          {params.value}
        </Box>
      )
    },
    {
      field: 'ipAddress',
      headerName: 'IP Address',
      width: 130
    },
    {
      field: 'statusCode',
      headerName: 'Status',
      width: 80,
      renderCell: (params) => (
        <Chip
          label={params.value}
          color={params.value === '200' ? 'success' : 'error'}
          size="small"
        />
      )
    },
    {
      field: 'actions',
      headerName: 'Details',
      width: 100,
      sortable: false,
      renderCell: (params) => (
        <Tooltip title="View Details">
          <IconButton
            size="small"
            onClick={() => setSelectedLog(params.row)}
          >
            <ViewIcon />
          </IconButton>
        </Tooltip>
      )
    }
  ];

  const filteredLogs = auditLogs.filter(log => {
    const matchesSearch = log.userName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         log.entityName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         log.action?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesAction = filterAction === 'all' || log.action === filterAction;
    const matchesEntity = filterEntity === 'all' || log.entityType === filterEntity;
    const matchesDateRange = (!startDate || new Date(log.timestamp) >= startDate) &&
                           (!endDate || new Date(log.timestamp) <= endDate);
    return matchesSearch && matchesAction && matchesEntity && matchesDateRange;
  });

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
          <Typography variant="h4">{t('audit.title')}</Typography>
          <Box>
            <Button
              variant="outlined"
              startIcon={<RefreshIcon />}
              onClick={fetchAuditLogs}
              sx={{ mr: 1 }}
            >
              {t('common.refresh')}
            </Button>
            <Button
              variant="outlined"
              startIcon={<DownloadIcon />}
              onClick={() => showSnackbar('Export feature coming soon')}
            >
              {t('common.export')}
            </Button>
          </Box>
        </Box>

        {/* Summary Cards */}
        <Grid container spacing={3} sx={{ mb: 3 }}>
          <Grid item xs={12} sm={6} md={3}>
            <Card>
              <CardContent>
                <Box display="flex" alignItems="center" justifyContent="space-between">
                  <Box>
                    <Typography color="textSecondary" gutterBottom variant="h6">
                      Total Logs
                    </Typography>
                    <Typography variant="h4">{auditLogs.length}</Typography>
                  </Box>
                  <Avatar sx={{ bgcolor: 'primary.main' }}>
                    <SecurityIcon />
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
                      Login Events
                    </Typography>
                    <Typography variant="h4" color="success.main">
                      {auditLogs.filter(log => log.action === 'LOGIN').length}
                    </Typography>
                  </Box>
                  <Avatar sx={{ bgcolor: 'success.main' }}>
                    <PersonIcon />
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
                      Data Changes
                    </Typography>
                    <Typography variant="h4" color="warning.main">
                      {auditLogs.filter(log => ['CREATE', 'UPDATE', 'DELETE'].includes(log.action)).length}
                    </Typography>
                  </Box>
                  <Avatar sx={{ bgcolor: 'warning.main' }}>
                    <BusinessIcon />
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
                      Failed Actions
                    </Typography>
                    <Typography variant="h4" color="error.main">
                      {auditLogs.filter(log => log.statusCode !== '200').length}
                    </Typography>
                  </Box>
                  <Avatar sx={{ bgcolor: 'error.main' }}>
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
            <Grid item xs={12} md={3}>
              <TextField
                fullWidth
                placeholder="Search logs..."
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
            <Grid item xs={12} md={2}>
              <FormControl fullWidth>
                <InputLabel>Action</InputLabel>
                <Select
                  value={filterAction}
                  label="Action"
                  onChange={(e) => setFilterAction(e.target.value)}
                >
                  <MenuItem value="all">All</MenuItem>
                  {actions.map(action => (
                    <MenuItem key={action} value={action}>
                      {action}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} md={2}>
              <FormControl fullWidth>
                <InputLabel>Entity</InputLabel>
                <Select
                  value={filterEntity}
                  label="Entity"
                  onChange={(e) => setFilterEntity(e.target.value)}
                >
                  <MenuItem value="all">All</MenuItem>
                  {entities.map(entity => (
                    <MenuItem key={entity} value={entity}>
                      {entity}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} md={2}>
              <DatePicker
                label="Start Date"
                value={startDate}
                onChange={setStartDate}
                renderInput={(params) => <TextField {...params} fullWidth />}
              />
            </Grid>
            <Grid item xs={12} md={2}>
              <DatePicker
                label="End Date"
                value={endDate}
                onChange={setEndDate}
                renderInput={(params) => <TextField {...params} fullWidth />}
              />
            </Grid>
          </Grid>
        </Paper>

        {/* Audit Logs Table */}
        <Paper sx={{ height: 600, width: '100%' }}>
          <DataGrid
            rows={filteredLogs}
            columns={columns}
            pageSize={10}
            rowsPerPageOptions={[5, 10, 25]}
            checkboxSelection
            disableSelectionOnClick
            loading={loading}
          />
        </Paper>

        {/* Log Details Dialog */}
        {selectedLog && (
          <Dialog open={!!selectedLog} onClose={() => setSelectedLog(null)} maxWidth="md" fullWidth>
            <DialogTitle>
              Audit Log Details
            </DialogTitle>
            <DialogContent>
              <Grid container spacing={3} sx={{ mt: 1 }}>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>Event Information</Typography>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Timestamp</Typography>
                    <Typography variant="body1">
                      {new Date(selectedLog.timestamp).toLocaleString()}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Action</Typography>
                    <Box display="flex" alignItems="center">
                      {getActionIcon(selectedLog.action)}
                      <Chip
                        label={selectedLog.action}
                        color={getActionColor(selectedLog.action)}
                        size="small"
                        sx={{ ml: 1 }}
                      />
                    </Box>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Entity Type</Typography>
                    <Typography variant="body1">{selectedLog.entityType}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Entity Name</Typography>
                    <Typography variant="body1">{selectedLog.entityName}</Typography>
                  </Box>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Typography variant="h6" gutterBottom>User & System Info</Typography>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">User</Typography>
                    <Typography variant="body1">{selectedLog.userName}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">IP Address</Typography>
                    <Typography variant="body1">{selectedLog.ipAddress}</Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">User Agent</Typography>
                    <Typography variant="body1" sx={{ wordBreak: 'break-all' }}>
                      {selectedLog.userAgent}
                    </Typography>
                  </Box>
                  <Box mb={2}>
                    <Typography variant="body2" color="textSecondary">Status Code</Typography>
                    <Chip
                      label={selectedLog.statusCode}
                      color={selectedLog.statusCode === '200' ? 'success' : 'error'}
                      size="small"
                    />
                  </Box>
                </Grid>
                {selectedLog.requestData && (
                  <Grid item xs={12}>
                    <Accordion>
                      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                        <Typography variant="h6">Request Data</Typography>
                      </AccordionSummary>
                      <AccordionDetails>
                        <Typography variant="body2" sx={{ fontFamily: 'monospace', whiteSpace: 'pre-wrap' }}>
                          {selectedLog.requestData}
                        </Typography>
                      </AccordionDetails>
                    </Accordion>
                  </Grid>
                )}
                {selectedLog.responseData && (
                  <Grid item xs={12}>
                    <Accordion>
                      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                        <Typography variant="h6">Response Data</Typography>
                      </AccordionSummary>
                      <AccordionDetails>
                        <Typography variant="body2" sx={{ fontFamily: 'monospace', whiteSpace: 'pre-wrap' }}>
                          {selectedLog.responseData}
                        </Typography>
                      </AccordionDetails>
                    </Accordion>
                  </Grid>
                )}
              </Grid>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setSelectedLog(null)}>Close</Button>
            </DialogActions>
          </Dialog>
        )}

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

export default AuditLogs;
