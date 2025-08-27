// API Gateway Configuration
export const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'https://localhost:7034/api';

// Service Endpoints
export const ENDPOINTS = {
  // Authentication
  LOGIN: '/identity/login',
  REGISTER: '/identity/register',
  
  // Admin Services
  USERS: '/admin/users',
  ROLES: '/admin/roles',
  PERMISSIONS: '/admin/permissions',
  
  // Supplier Services
  SUPPLIERS: '/suppliers',
  OFFERS: '/offers',
  LOOKUP: '/lookup',
  
  // Order Services
  ORDERS: '/orders',
  
  // Payment Services
  PAYMENTS: '/payments',
  
  // Accounting Services
  ACCOUNTING: '/accounting',
  
  // Inventory Services
  INVENTORY: '/inventory',
  
  // Audit Services
  AUDIT: '/audit'
};

// Role Configuration
export const ROLES = {
  SUPER_ADMIN: 'SuperAdmin',
  ADMIN: 'Admin',
  MANAGER: 'Manager',
  USER: 'User'
};

// Permission Configuration
export const PERMISSIONS = {
  // User Management
  VIEW_USERS: 'view_users',
  CREATE_USERS: 'create_users',
  UPDATE_USERS: 'update_users',
  DELETE_USERS: 'delete_users',
  
  // Supplier Management
  VIEW_SUPPLIERS: 'view_suppliers',
  CREATE_SUPPLIERS: 'create_suppliers',
  UPDATE_SUPPLIERS: 'update_suppliers',
  DELETE_SUPPLIERS: 'delete_suppliers',
  
  // Order Management
  VIEW_ORDERS: 'view_orders',
  CREATE_ORDERS: 'create_orders',
  UPDATE_ORDERS: 'update_orders',
  DELETE_ORDERS: 'delete_orders',
  
  // Audit Logs
  VIEW_AUDIT_LOGS: 'view_audit_logs',
  DELETE_AUDIT_LOGS: 'delete_audit_logs'
};