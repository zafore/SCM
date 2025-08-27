import React from "react";
import { useTranslation } from "react-i18next";
import { 
  Box, 
  Typography, 
  Paper, 
  Grid, 
  Card, 
  CardContent, 
  Avatar,
  LinearProgress,
  Chip,
  Button,
  IconButton,
  Divider
} from "@mui/material";
import {
  TrendingUp as TrendingUpIcon,
  TrendingDown as TrendingDownIcon,
  Business as BusinessIcon,
  ShoppingCart as ShoppingCartIcon,
  Inventory as InventoryIcon,
  Payment as PaymentIcon,
  Add as AddIcon,
  MoreVert as MoreVertIcon,
  ArrowForward as ArrowForwardIcon
} from "@mui/icons-material";

const Dashboard = () => {
  const { t } = useTranslation();

  const statsData = [
    {
      title: t('common.suppliers'),
      value: '24',
      change: '+12%',
      trend: 'up',
      icon: <BusinessIcon />,
      color: 'primary',
      bgColor: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)'
    },
    {
      title: t('common.orders'),
      value: '156',
      change: '+8%',
      trend: 'up',
      icon: <ShoppingCartIcon />,
      color: 'success',
      bgColor: 'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)'
    },
    {
      title: t('common.inventory'),
      value: '89',
      change: '-3%',
      trend: 'down',
      icon: <InventoryIcon />,
      color: 'warning',
      bgColor: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)'
    },
    {
      title: t('common.payments'),
      value: '$45,678',
      change: '+15%',
      trend: 'up',
      icon: <PaymentIcon />,
      color: 'info',
      bgColor: 'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)'
    }
  ];

  const recentActivities = [
    { action: t('dashboard.newSupplierAdded'), time: '2 hours ago', type: 'success' },
    { action: t('dashboard.orderCompleted'), time: '4 hours ago', type: 'info' },
    { action: t('dashboard.paymentReceived'), time: '6 hours ago', type: 'success' },
    { action: t('dashboard.inventoryUpdated'), time: '8 hours ago', type: 'warning' },
  ];

  return (
    <Box sx={{ p: 3 }}>
      {/* Header */}
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" gutterBottom sx={{ fontWeight: 700, color: 'text.primary' }}>
          {t('dashboard.title')}
        </Typography>
        <Typography variant="h6" color="text.secondary" sx={{ mb: 2 }}>
          {t('dashboard.welcome')} - {new Date().toLocaleDateString()}
        </Typography>
        <LinearProgress 
          variant="determinate" 
          value={75} 
          sx={{ 
            height: 4, 
            borderRadius: 2,
            background: 'linear-gradient(90deg, #667eea 0%, #764ba2 100%)'
          }} 
        />
      </Box>

      {/* Stats Cards */}
      <Grid container spacing={3} sx={{ mb: 4 }}>
        {statsData.map((stat, index) => (
          <Grid item xs={12} sm={6} md={3} key={index}>
            <Card 
              sx={{ 
                position: 'relative',
                overflow: 'hidden',
                background: stat.bgColor,
                color: 'white',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  transition: 'transform 0.3s ease-in-out'
                }
              }}
            >
              <CardContent>
                <Box display="flex" alignItems="center" justifyContent="space-between" sx={{ mb: 2 }}>
                  <Avatar 
                    sx={{ 
                      bgcolor: 'rgba(255,255,255,0.2)', 
                      width: 48, 
                      height: 48,
                      backdropFilter: 'blur(10px)'
                    }}
                  >
                    {stat.icon}
                  </Avatar>
                  <IconButton size="small" sx={{ color: 'white' }}>
                    <MoreVertIcon />
                  </IconButton>
                </Box>
                
                <Typography variant="h4" sx={{ fontWeight: 700, mb: 1 }}>
                  {stat.value}
                </Typography>
                
                <Typography variant="body2" sx={{ opacity: 0.9, mb: 1 }}>
                  {stat.title}
                </Typography>
                
                <Box display="flex" alignItems="center" gap={1}>
                  {stat.trend === 'up' ? (
                    <TrendingUpIcon sx={{ fontSize: 16 }} />
                  ) : (
                    <TrendingDownIcon sx={{ fontSize: 16 }} />
                  )}
                  <Typography variant="body2" sx={{ fontWeight: 500 }}>
                    {stat.change}
                  </Typography>
                </Box>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>

      {/* Content Grid */}
      <Grid container spacing={3}>
        {/* Recent Activity */}
        <Grid item xs={12} md={8}>
          <Card sx={{ height: '100%' }}>
            <CardContent>
              <Box display="flex" alignItems="center" justifyContent="space-between" sx={{ mb: 3 }}>
                <Typography variant="h6" sx={{ fontWeight: 600 }}>
                  {t('dashboard.recentActivity')}
                </Typography>
                <Button 
                  variant="outlined" 
                  size="small" 
                  endIcon={<ArrowForwardIcon />}
                >
                  {t('common.view')}
                </Button>
              </Box>
              
              <Box>
                {recentActivities.map((activity, index) => (
                  <Box key={index}>
                    <Box display="flex" alignItems="center" justifyContent="space-between" sx={{ py: 2 }}>
                      <Box display="flex" alignItems="center" gap={2}>
                        <Avatar 
                          sx={{ 
                            width: 32, 
                            height: 32,
                            bgcolor: activity.type === 'success' ? 'success.main' : 
                                    activity.type === 'warning' ? 'warning.main' : 'info.main'
                          }}
                        >
                          {activity.type === 'success' ? '✓' : 
                           activity.type === 'warning' ? '⚠' : 'ℹ'}
                        </Avatar>
                        <Box>
                          <Typography variant="body1" sx={{ fontWeight: 500 }}>
                            {activity.action}
                          </Typography>
                          <Typography variant="body2" color="text.secondary">
                            {activity.time}
                          </Typography>
                        </Box>
                      </Box>
                      <Chip 
                        label={activity.type} 
                        size="small" 
                        color={activity.type}
                        variant="outlined"
                      />
                    </Box>
                    {index < recentActivities.length - 1 && <Divider />}
                  </Box>
                ))}
              </Box>
            </CardContent>
          </Card>
        </Grid>

        {/* Quick Actions */}
        <Grid item xs={12} md={4}>
          <Card sx={{ height: '100%' }}>
            <CardContent>
              <Typography variant="h6" sx={{ fontWeight: 600, mb: 3 }}>
                {t('dashboard.quickActions')}
              </Typography>
              
              <Box display="flex" flexDirection="column" gap={2}>
                <Button 
                  variant="contained" 
                  startIcon={<AddIcon />}
                  fullWidth
                  sx={{ justifyContent: 'flex-start', py: 1.5 }}
                >
                  {t('suppliers.addSupplier')}
                </Button>
                
                <Button 
                  variant="outlined" 
                  startIcon={<ShoppingCartIcon />}
                  fullWidth
                  sx={{ justifyContent: 'flex-start', py: 1.5 }}
                >
                  {t('orders.addOrder')}
                </Button>
                
                <Button 
                  variant="outlined" 
                  startIcon={<InventoryIcon />}
                  fullWidth
                  sx={{ justifyContent: 'flex-start', py: 1.5 }}
                >
                  {t('inventory.addItem')}
                </Button>
                
                <Button 
                  variant="outlined" 
                  startIcon={<PaymentIcon />}
                  fullWidth
                  sx={{ justifyContent: 'flex-start', py: 1.5 }}
                >
                  {t('payments.addPayment')}
                </Button>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>
  );
};

export default Dashboard;