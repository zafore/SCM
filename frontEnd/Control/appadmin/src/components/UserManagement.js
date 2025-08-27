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
  Tooltip
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Search as SearchIcon,
  Person as PersonIcon,
  Email as EmailIcon,
  AdminPanelSettings as AdminIcon,
  SupervisorAccount as ManagerIcon,
  Person as UserIcon,
  Block as BlockIcon,
  CheckCircle as CheckCircleIcon
} from '@mui/icons-material';
import { DataGrid } from '@mui/x-data-grid';
import api from '../services/api';
import { ENDPOINTS, ROLES } from '../config';

const UserManagement = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingUser, setEditingUser] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterRole, setFilterRole] = useState('all');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    role: ROLES.USER,
    isActive: true
  });

  const roleIcons = {
    [ROLES.SUPER_ADMIN]: <AdminIcon />,
    [ROLES.ADMIN]: <AdminIcon />,
    [ROLES.MANAGER]: <ManagerIcon />,
    [ROLES.USER]: <UserIcon />
  };

  const roleColors = {
    [ROLES.SUPER_ADMIN]: 'error',
    [ROLES.ADMIN]: 'warning',
    [ROLES.MANAGER]: 'info',
    [ROLES.USER]: 'default'
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      // Mock data since we don't have user management endpoint yet
      const mockUsers = [
        {
          id: 1,
          name: 'John Doe',
          email: 'john.doe@company.com',
          role: ROLES.ADMIN,
          isActive: true,
          lastLogin: '2024-01-15T10:30:00Z',
          createdAt: '2024-01-01T00:00:00Z'
        },
        {
          id: 2,
          name: 'Jane Smith',
          email: 'jane.smith@company.com',
          role: ROLES.MANAGER,
          isActive: true,
          lastLogin: '2024-01-14T15:45:00Z',
          createdAt: '2024-01-02T00:00:00Z'
        },
        {
          id: 3,
          name: 'Mike Johnson',
          email: 'mike.johnson@company.com',
          role: ROLES.USER,
          isActive: false,
          lastLogin: '2024-01-10T09:20:00Z',
          createdAt: '2024-01-03T00:00:00Z'
        }
      ];
      setUsers(mockUsers);
    } catch (error) {
      console.error('Error fetching users:', error);
      showSnackbar('Error fetching users', 'error');
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

  const handleOpenDialog = (user = null) => {
    if (user) {
      setEditingUser(user);
      setFormData({
        name: user.name || '',
        email: user.email || '',
        password: '',
        role: user.role || ROLES.USER,
        isActive: user.isActive !== undefined ? user.isActive : true
      });
    } else {
      setEditingUser(null);
      setFormData({
        name: '',
        email: '',
        password: '',
        role: ROLES.USER,
        isActive: true
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingUser(null);
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async () => {
    try {
      // Simulate API call
      showSnackbar(editingUser ? 'User updated successfully' : 'User created successfully');
      handleCloseDialog();
      fetchUsers();
    } catch (error) {
      console.error('Error saving user:', error);
      showSnackbar('Error saving user', 'error');
    }
  };

  const handleDelete = async (userId) => {
    if (window.confirm('Are you sure you want to delete this user?')) {
      try {
        // Simulate API call
        showSnackbar('User deleted successfully');
        fetchUsers();
      } catch (error) {
        console.error('Error deleting user:', error);
        showSnackbar('Error deleting user', 'error');
      }
    }
  };

  const handleToggleStatus = async (userId, currentStatus) => {
    try {
      // Simulate API call
      showSnackbar(`User ${currentStatus ? 'deactivated' : 'activated'} successfully`);
      fetchUsers();
    } catch (error) {
      console.error('Error updating user status:', error);
      showSnackbar('Error updating user status', 'error');
    }
  };

  const filteredUsers = users.filter(user => {
    const matchesSearch = user.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         user.email?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesRole = filterRole === 'all' || user.role === filterRole;
    return matchesSearch && matchesRole;
  });

  const columns = [
    {
      field: 'name',
      headerName: 'Name',
      width: 200,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          <Avatar sx={{ mr: 2, bgcolor: 'primary.main' }}>
            <PersonIcon />
          </Avatar>
          <Box>
            <Typography variant="subtitle2">{params.value}</Typography>
            <Typography variant="caption" color="textSecondary">
              {params.row.email}
            </Typography>
          </Box>
        </Box>
      )
    },
    {
      field: 'role',
      headerName: 'Role',
      width: 150,
      renderCell: (params) => (
        <Box display="flex" alignItems="center">
          {roleIcons[params.value]}
          <Chip
            label={params.value}
            color={roleColors[params.value]}
            size="small"
            sx={{ ml: 1 }}
          />
        </Box>
      )
    },
    {
      field: 'isActive',
      headerName: 'Status',
      width: 120,
      renderCell: (params) => (
        <Chip
          label={params.value ? 'Active' : 'Inactive'}
          color={params.value ? 'success' : 'error'}
          size="small"
        />
      )
    },
    {
      field: 'lastLogin',
      headerName: 'Last Login',
      width: 150,
      renderCell: (params) => (
        params.value ? new Date(params.value).toLocaleDateString() : 'Never'
      )
    },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 150,
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
          <Tooltip title={params.row.isActive ? 'Deactivate' : 'Activate'}>
            <IconButton
              size="small"
              onClick={() => handleToggleStatus(params.row.id, params.row.isActive)}
              color={params.row.isActive ? 'warning' : 'success'}
            >
              {params.row.isActive ? <BlockIcon /> : <CheckCircleIcon />}
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
        <Typography variant="h4">User Management</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpenDialog()}
        >
          Add User
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
                    Total Users
                  </Typography>
                  <Typography variant="h4">{users.length}</Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'primary.main' }}>
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
                    Active Users
                  </Typography>
                  <Typography variant="h4" color="success.main">
                    {users.filter(u => u.isActive).length}
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
                    Administrators
                  </Typography>
                  <Typography variant="h4" color="warning.main">
                    {users.filter(u => u.role === ROLES.ADMIN || u.role === ROLES.SUPER_ADMIN).length}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'warning.main' }}>
                  <AdminIcon />
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
                    Managers
                  </Typography>
                  <Typography variant="h4" color="info.main">
                    {users.filter(u => u.role === ROLES.MANAGER).length}
                  </Typography>
                </Box>
                <Avatar sx={{ bgcolor: 'info.main' }}>
                  <ManagerIcon />
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
              placeholder="Search users..."
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
              <InputLabel>Role</InputLabel>
              <Select
                value={filterRole}
                label="Role"
                onChange={(e) => setFilterRole(e.target.value)}
              >
                <MenuItem value="all">All</MenuItem>
                <MenuItem value={ROLES.SUPER_ADMIN}>Super Admin</MenuItem>
                <MenuItem value={ROLES.ADMIN}>Admin</MenuItem>
                <MenuItem value={ROLES.MANAGER}>Manager</MenuItem>
                <MenuItem value={ROLES.USER}>User</MenuItem>
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      {/* Users Table */}
      <Paper sx={{ height: 600, width: '100%' }}>
        <DataGrid
          rows={users}
          columns={columns}
          pageSize={10}
          rowsPerPageOptions={[5, 10, 25]}
          checkboxSelection
          disableSelectionOnClick
          loading={loading}
        />
      </Paper>

      {/* Add/Edit Dialog */}
      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>
          {editingUser ? 'Edit User' : 'Add New User'}
        </DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Full Name"
                name="name"
                value={formData.name}
                onChange={handleInputChange}
                required
              />
            </Grid>
            <Grid item xs={12}>
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
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Password"
                name="password"
                type="password"
                value={formData.password}
                onChange={handleInputChange}
                required={!editingUser}
                helperText={editingUser ? "Leave blank to keep current password" : ""}
              />
            </Grid>
            <Grid item xs={12}>
              <FormControl fullWidth>
                <InputLabel>Role</InputLabel>
                <Select
                  name="role"
                  value={formData.role}
                  label="Role"
                  onChange={handleInputChange}
                >
                  <MenuItem value={ROLES.USER}>User</MenuItem>
                  <MenuItem value={ROLES.MANAGER}>Manager</MenuItem>
                  <MenuItem value={ROLES.ADMIN}>Admin</MenuItem>
                  <MenuItem value={ROLES.SUPER_ADMIN}>Super Admin</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancel</Button>
          <Button onClick={handleSubmit} variant="contained">
            {editingUser ? 'Update' : 'Create'}
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

export default UserManagement;
