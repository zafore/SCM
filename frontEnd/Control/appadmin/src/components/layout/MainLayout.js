import React, { useState } from 'react';
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  List,
  Typography,
  Divider,
  IconButton,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Avatar,
  Menu,
  MenuItem,
  Badge,
  useTheme,
  useMediaQuery
} from '@mui/material';
import {
  Menu as MenuIcon,
  Dashboard as DashboardIcon,
  People as PeopleIcon,
  Business as BusinessIcon,
  ShoppingCart as ShoppingCartIcon,
  Payment as PaymentIcon,
  Assessment as AssessmentIcon,
  Security as SecurityIcon,
  Notifications as NotificationsIcon,
  AccountCircle as AccountCircleIcon,
  Logout as LogoutIcon,
  Settings as SettingsIcon,
  Inventory as InventoryIcon,
  Assignment as AssignmentIcon
} from '@mui/icons-material';
import { useNavigate, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import AuthService from '../../services/AuthServices';
import { ROLES } from '../../config';
import LanguageSwitcher from '../LanguageSwitcher';

const drawerWidth = 240;

const getMenuItems = (t) => [
  {
    text: t('common.dashboard'),
    icon: <DashboardIcon />,
    path: '/dashboard',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER, ROLES.USER]
  },
  {
    text: t('common.suppliers'),
    icon: <BusinessIcon />,
    path: '/suppliers',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  },
  {
    text: t('common.orders'),
    icon: <ShoppingCartIcon />,
    path: '/orders',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER, ROLES.USER]
  },
  {
    text: t('common.inventory'),
    icon: <InventoryIcon />,
    path: '/inventory',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  },
  {
    text: t('common.payments'),
    icon: <PaymentIcon />,
    path: '/payments',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  },
  {
    text: t('common.accounting'),
    icon: <AssessmentIcon />,
    path: '/accounting',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  },
  {
    text: t('common.contracts'),
    icon: <AssignmentIcon />,
    path: '/contracts',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  },
  {
    text: t('users.title'),
    icon: <PeopleIcon />,
    path: '/users',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN]
  },
  {
    text: t('common.audit'),
    icon: <SecurityIcon />,
    path: '/audit',
    roles: [ROLES.SUPER_ADMIN, ROLES.ADMIN, ROLES.MANAGER]
  }
];

const MainLayout = ({ children }) => {
  const { t, i18n } = useTranslation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const [mobileOpen, setMobileOpen] = useState(false);
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  const user = AuthService.getCurrentUser();
  const userRole = user?.role || ROLES.USER;
  
  const menuItems = getMenuItems(t);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const handleProfileMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleProfileMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    AuthService.logout();
    navigate('/login');
    handleProfileMenuClose();
  };

  const handleSettings = () => {
    navigate('/settings');
    handleProfileMenuClose();
  };

  const filteredMenuItems = menuItems.filter(item => 
    item.roles.includes(userRole)
  );

  const drawer = (
    <div>
      <Toolbar sx={{ 
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        color: 'white',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
      }}>
        <Typography variant="h6" noWrap component="div" sx={{ fontWeight: 700, fontSize: '1.2rem' }}>
          SCM Admin
        </Typography>
      </Toolbar>
      <Divider />
      <List sx={{ px: 1, py: 2 }}>
        {filteredMenuItems.map((item) => (
          <ListItem key={item.text} disablePadding sx={{ mb: 0.5 }}>
            <ListItemButton
              selected={location.pathname === item.path}
              onClick={() => {
                navigate(item.path);
                if (isMobile) {
                  setMobileOpen(false);
                }
              }}
              sx={{
                borderRadius: 2,
                mx: 1,
                '&.Mui-selected': {
                  background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                  color: 'white',
                  '&:hover': {
                    background: 'linear-gradient(135deg, #5a6fd8 0%, #6a4190 100%)',
                  },
                  '& .MuiListItemIcon-root': {
                    color: 'white',
                  },
                },
                '&:hover': {
                  backgroundColor: 'rgba(102, 126, 234, 0.1)',
                  borderRadius: 2,
                },
              }}
            >
              <ListItemIcon sx={{ minWidth: 40 }}>
                {item.icon}
              </ListItemIcon>
              <ListItemText 
                primary={item.text} 
                primaryTypographyProps={{ 
                  fontWeight: location.pathname === item.path ? 600 : 500,
                  fontSize: '0.9rem'
                }} 
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </div>
  );

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{
          width: { md: `calc(100% - ${drawerWidth}px)` },
          ml: { md: `${drawerWidth}px` },
          background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
          backdropFilter: 'blur(20px)',
          borderBottom: '1px solid rgba(255, 255, 255, 0.1)',
          boxShadow: '0 4px 20px rgba(0, 0, 0, 0.1)',
        }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { md: 'none' } }}
          >
            <MenuIcon />
          </IconButton>
          
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1, fontWeight: 600 }}>
            {filteredMenuItems.find(item => item.path === location.pathname)?.text || t('common.dashboard')}
          </Typography>

          <LanguageSwitcher />

          <IconButton color="inherit">
            <Badge badgeContent={4} color="error">
              <NotificationsIcon />
            </Badge>
          </IconButton>

          <IconButton
            size="large"
            edge="end"
            aria-label="account of current user"
            aria-controls="primary-search-account-menu"
            aria-haspopup="true"
            onClick={handleProfileMenuOpen}
            color="inherit"
            sx={{
              '&:hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.1)',
              }
            }}
          >
            <Avatar sx={{ 
              width: 36, 
              height: 36, 
              bgcolor: 'rgba(255, 255, 255, 0.2)',
              border: '2px solid rgba(255, 255, 255, 0.3)',
              fontWeight: 600
            }}>
              {user?.name?.charAt(0)?.toUpperCase() || 'U'}
            </Avatar>
          </IconButton>
        </Toolbar>
      </AppBar>

      <Box
        component="nav"
        sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}
        aria-label="mailbox folders"
      >
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true,
          }}
          sx={{
            display: { xs: 'block', md: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', md: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
          open
        >
          {drawer}
        </Drawer>
      </Box>

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          width: { md: `calc(100% - ${drawerWidth}px)` },
          mt: 8
        }}
      >
        {children}
      </Box>

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleProfileMenuClose}
        onClick={handleProfileMenuClose}
        PaperProps={{
          elevation: 0,
          sx: {
            overflow: 'visible',
            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
            mt: 1.5,
            '& .MuiAvatar-root': {
              width: 32,
              height: 32,
              ml: -0.5,
              mr: 1,
            },
            '&:before': {
              content: '""',
              display: 'block',
              position: 'absolute',
              top: 0,
              right: 14,
              width: 10,
              height: 10,
              bgcolor: 'background.paper',
              transform: 'translateY(-50%) rotate(45deg)',
              zIndex: 0,
            },
          },
        }}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem onClick={handleProfileMenuClose}>
          <Avatar /> {t('navigation.profile')}
        </MenuItem>
        <MenuItem onClick={handleSettings}>
          <ListItemIcon>
            <SettingsIcon fontSize="small" />
          </ListItemIcon>
          {t('navigation.settings')}
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <LogoutIcon fontSize="small" />
          </ListItemIcon>
          {t('common.logout')}
        </MenuItem>
      </Menu>
    </Box>
  );
};

export default MainLayout;
