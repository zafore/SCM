# 🎛️ SCM Admin Dashboard

A modern, responsive React admin dashboard for Supply Chain Management (SCM) system with role-based access control and comprehensive business management features.

## 🚀 Features

### 🔐 **Authentication & Authorization**
- JWT-based authentication via API Gateway
- Role-based access control (SuperAdmin, Admin, Manager, User)
- Protected routes with automatic redirects
- Session management with token expiration handling

### 📊 **Dashboard**
- Real-time system statistics
- Interactive charts and graphs
- Quick action cards
- Recent activities feed
- System status monitoring

### 🏢 **Supplier Management**
- Complete CRUD operations for suppliers
- Advanced search and filtering
- Supplier rating system
- Contact information management
- Status tracking (Active, Inactive, Pending)

### 📦 **Order Management**
- Order creation and tracking
- Status workflow (Pending → Confirmed → Processing → Shipped → Delivered)
- Order details with stepper progress
- Priority management (High, Medium, Low)
- Integration with supplier data

### 📋 **Contract Management**
- Contract lifecycle management
- Multiple contract types (Service, Supply, Maintenance, etc.)
- Document management
- Renewal tracking
- Status monitoring

### 📦 **Inventory Management**
- Stock level monitoring
- Low stock alerts
- Category management
- Value tracking
- Stock status indicators

### 💰 **Payments & Accounting**
- Payment processing tracking
- Financial transaction management
- Income vs Expense tracking
- Net income calculations
- Payment method tracking

### 👥 **User Management**
- User creation and management
- Role assignment
- User status control (Active/Inactive)
- Last login tracking
- Permission management

### 🔍 **Audit Logs**
- Comprehensive activity tracking
- User action monitoring
- System event logging
- Advanced filtering and search
- Export capabilities

## 🛠️ Technology Stack

- **Frontend**: React 19.1.1
- **UI Framework**: Material-UI (MUI) 5.14.20
- **Data Grid**: MUI X Data Grid 6.18.2
- **Charts**: Recharts 2.8.0
- **Date Pickers**: MUI X Date Pickers 6.18.2
- **HTTP Client**: Axios 1.11.0
- **Routing**: React Router DOM 7.8.1
- **Authentication**: JWT with jwt-decode 4.0.0
- **Date Handling**: date-fns 2.30.0

## 📁 Project Structure

```
src/
├── components/
│   ├── layout/
│   │   ├── MainLayout.js          # Main application layout
│   │   └── ProtectedRoute.js      # Route protection component
│   ├── Dashboard.js               # Main dashboard
│   ├── Suppliers.js               # Supplier management
│   ├── Orders.js                  # Order management
│   ├── Contracts.js               # Contract management
│   ├── Inventory.js               # Inventory management
│   ├── Payments.js                # Payment management
│   ├── Accounting.js              # Accounting & finance
│   ├── UserManagement.js          # User management
│   ├── AuditLogs.js               # Audit logs viewer
│   ├── Login.js                   # Login component
│   └── Unauthorized.js            # Unauthorized access page
├── services/
│   ├── api.js                     # Axios configuration
│   └── AuthServices.js            # Authentication service
├── config.js                      # Application configuration
└── App.js                         # Main application component
```

## 🚀 Getting Started

### Prerequisites
- Node.js 16+ 
- npm or yarn
- API Gateway running on https://localhost:7034

### Installation

1. **Install dependencies**:
   ```bash
   npm install
   ```

2. **Configure environment**:
   Create `.env` file in the root directory:
   ```env
   REACT_APP_API_BASE_URL=https://localhost:7034/api
   ```

3. **Start development server**:
   ```bash
   npm start
   ```

4. **Open browser**:
   Navigate to `http://localhost:3000`

### Build for Production

```bash
npm run build
```

## 🔧 Configuration

### API Gateway Integration

The dashboard is configured to work with the API Gateway at `https://localhost:7034/api`. Update the configuration in `src/config.js`:

```javascript
export const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'https://localhost:7034/api';
```

### Role Configuration

Roles are defined in `src/config.js`:

```javascript
export const ROLES = {
  SUPER_ADMIN: 'SuperAdmin',
  ADMIN: 'Admin',
  MANAGER: 'Manager',
  USER: 'User'
};
```

### Permission System

Permissions are configured for different user roles:

- **SuperAdmin**: Full access to all features
- **Admin**: Access to most features except user management
- **Manager**: Access to operational features (suppliers, orders, inventory)
- **User**: Limited access to view-only features

## 🎨 UI Components

### Material-UI Theme
- Custom theme with primary/secondary colors
- Responsive design for mobile and desktop
- Dark/light mode support (configurable)

### Data Grid Features
- Sorting and filtering
- Pagination
- Column resizing
- Row selection
- Export capabilities

### Charts and Visualizations
- Line charts for sales trends
- Pie charts for status distribution
- Bar charts for comparisons
- Real-time data updates

## 🔐 Security Features

### Authentication
- JWT token-based authentication
- Automatic token refresh
- Secure token storage
- Session timeout handling

### Authorization
- Route-level protection
- Component-level access control
- API request authorization
- Role-based UI rendering

### Data Protection
- Input validation
- XSS protection
- CSRF protection
- Secure API communication

## 📱 Responsive Design

- Mobile-first approach
- Responsive navigation drawer
- Adaptive layouts
- Touch-friendly interfaces
- Cross-browser compatibility

## 🧪 Testing

```bash
# Run tests
npm test

# Run tests with coverage
npm run test:coverage

# Run e2e tests
npm run test:e2e
```

## 📦 Deployment

### Docker Deployment

```dockerfile
FROM node:16-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
EXPOSE 3000
CMD ["npm", "start"]
```

### Environment Variables

```env
REACT_APP_API_BASE_URL=https://your-api-gateway.com/api
REACT_APP_ENVIRONMENT=production
```

## 🔄 API Integration

### Endpoints Used

- **Authentication**: `/api/identity/login`
- **Suppliers**: `/api/suppliers`
- **Orders**: `/api/orders`
- **Payments**: `/api/payments`
- **Accounting**: `/api/accounting`
- **Inventory**: `/api/inventory`
- **Audit**: `/api/audit`

### Error Handling

- Global error boundary
- API error handling
- User-friendly error messages
- Retry mechanisms
- Offline support

## 🚀 Performance Optimization

- Code splitting
- Lazy loading
- Memoization
- Virtual scrolling
- Image optimization
- Bundle analysis

## 📊 Monitoring & Analytics

- User activity tracking
- Performance metrics
- Error logging
- Usage analytics
- Audit trail

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For support and questions:
- Check the documentation
- Review the troubleshooting guide
- Contact the development team
- Submit an issue on GitHub

---

## 🎯 Demo Credentials

```
Admin: admin@example.com / Admin123!
Manager: manager@example.com / Manager123!
User: user@example.com / User123!
```

## 🔗 Related Documentation

- [API Gateway Documentation](../APIGateWay/README.md)
- [Microservices Documentation](../MICROSERVICES_FIXES.md)
- [Audit System Documentation](../APIGateWay/AUDIT_SYSTEM.md)
- [Quick Start Guide](../QUICK_START.md)