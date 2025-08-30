# SCM Admin Application

A modern Supply Chain Management (SCM) administration application built with React and Bootstrap.

## Features

- **Modern UI**: Clean, responsive design using Bootstrap 5
- **Authentication**: Secure login system with role-based access control
- **Dashboard**: Comprehensive overview with statistics and quick actions
- **Lookup Management**: Manage categories, suppliers, tags, and currencies
- **Currency Integration**: Real-time currency exchange rates from API gateway
- **Multi-language Support**: English and Arabic localization
- **Responsive Design**: Works seamlessly on desktop and mobile devices

## Technology Stack

- **Frontend**: React 19
- **Styling**: Bootstrap 5 + Custom CSS
- **Routing**: React Router DOM
- **State Management**: React Hooks
- **Internationalization**: i18next
- **HTTP Client**: Axios
- **Build Tool**: Create React App

## Key Components

### Layout
- **MainLayout**: Responsive sidebar navigation with user authentication
- **ProtectedRoute**: Role-based route protection
- **LanguageSwitcher**: Multi-language support

### Pages
- **Dashboard**: Overview with statistics and quick actions
- **LookupManagement**: CRUD operations for reference data
- **Login**: Secure authentication interface
- **Suppliers**: Supplier management
- **Orders**: Order tracking and management
- **Inventory**: Stock management
- **Payments**: Payment processing
- **Accounting**: Financial management
- **Contracts**: Contract management
- **UserManagement**: User administration
- **AuditLogs**: System audit trail

## Getting Started

### Prerequisites
- Node.js 18+ 
- npm or yarn

### Installation
1. Clone the repository
2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) in your browser

### Build for Production
```bash
npm run build
```

## Project Structure

```
src/
├── components/          # React components
│   ├── layout/         # Layout components
│   ├── Dashboard.js    # Main dashboard
│   ├── Login.js        # Authentication
│   └── ...
├── services/           # API services
├── i18n/              # Internationalization
├── config.js          # Configuration
└── App.js             # Main application
```

## Features

### Lookup Management
- **Categories**: Product and service categories
- **Suppliers**: Vendor information and ratings
- **Tags**: Flexible labeling system
- **Currencies**: Real-time exchange rates with API integration

### Currency API Integration
The application integrates with external currency APIs to provide real-time exchange rates. Features include:
- Automatic currency fetching
- Fallback data for offline scenarios
- Refresh functionality
- Rate display and management

### Responsive Design
- Mobile-first approach
- Collapsible sidebar navigation
- Touch-friendly interface
- Optimized for all screen sizes

## Customization

### Styling
- Custom CSS variables for consistent theming
- Bootstrap utility classes for rapid development
- Responsive breakpoints for mobile optimization

### Adding New Features
1. Create new component in `src/components/`
2. Add route in `src/App.js`
3. Update navigation in `src/components/layout/MainLayout.js`
4. Add translations in `src/i18n/locales/`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please contact the development team.