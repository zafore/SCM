# Backend Configuration Guide

## 🔧 **Current API Configuration**

The frontend is currently configured to connect to:
- **Default URL**: `https://localhost:7034/api`
- **Environment Variable**: `VITE_API_BASE_URL`

## 🚀 **How to Configure Your Backend**

### **Option 1: Environment Variable (Recommended)**

Create a `.env` file in the root directory:

```bash
# .env
VITE_API_BASE_URL=http://localhost:YOUR_BACKEND_PORT
```

### **Option 2: Update Default Configuration**

Edit `src/api/config/endpoints.js` and change the default URL:

```javascript
export const API_CONFIG = {
  BASE_URL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:YOUR_BACKEND_PORT',
  // ... other config
};
```

## 📋 **Common Backend Ports**

| Framework | Default Port | URL |
|-----------|-------------|-----|
| .NET Core | 5000 | `http://localhost:5000` |
| **.NET Core (HTTPS)** | **5001** | **`https://localhost:5001`** |
| Express.js | 3000 | `http://localhost:3000` |
| Flask | 5000 | `http://localhost:5000` |
| Django | 8000 | `http://localhost:8000` |
| Spring Boot | 8080 | `http://localhost:8080` |

## 🔍 **How to Find Your Backend Port**

1. **Check your backend console output** - it usually shows the port
2. **Check Postman** - look at the URL you're using successfully
3. **Check your backend configuration files**

## 🧪 **Test Your Configuration**

1. **Create `.env` file** with your backend URL
2. **Restart the frontend** (`npm start`)
3. **Try logging in** with your backend credentials
4. **Check browser console** for connection status

## 📝 **Example Configuration**

If your backend runs on HTTPS port 7034:

```bash
# .env
VITE_API_BASE_URL=https://localhost:7034/api
```

If your backend runs on HTTP port 3000:

```bash
# .env
VITE_API_BASE_URL=http://localhost:3000
```

## 🎯 **Demo Mode Fallback**

If the backend is not available, the app will automatically:
- ✅ Fall back to demo mode
- ✅ Use mock data
- ✅ Show demo credentials
- ✅ Display demo status indicator

## 🔑 **Demo Credentials**

You can always use these demo credentials:
- **Username**: `admin@test.com`
- **Password**: `Admin123!`

Or:
- **Username**: `admin`
- **Password**: `admin`

## 🔒 **HTTPS Configuration**

### **Important Notes:**
- **HTTPS Backend**: If your backend uses HTTPS (like .NET Core with SSL), use `https://` in the URL
- **HTTP Backend**: If your backend uses HTTP, use `http://` in the URL
- **Mixed Content**: Browsers block HTTP requests from HTTPS pages, so match the protocols

### **Common HTTPS Ports:**
- **.NET Core HTTPS**: `https://localhost:5001`
- **Express.js HTTPS**: `https://localhost:3001`
- **Spring Boot HTTPS**: `https://localhost:8443`

## 🚨 **Troubleshooting**

### **If login still doesn't work:**

1. **Check CORS** - Make sure your backend allows requests from `http://localhost:5174`
2. **Check API endpoints** - Verify the login endpoint matches `/identity/login`
3. **Check network tab** - Look for failed requests in browser dev tools
4. **Check backend logs** - Look for incoming requests

### **Common Issues:**

- **CORS Error**: Backend needs to allow frontend origin
- **404 Error**: Wrong API endpoint path
- **Network Error**: Wrong port or backend not running
- **500 Error**: Backend server error
- **HTTPS/HTTP Mismatch**: Frontend trying HTTP but backend is HTTPS (or vice versa)

## 📞 **Need Help?**

1. **Check your backend port** in Postman
2. **Update the configuration** above
3. **Restart the frontend**
4. **Try logging in again**

The app will automatically fall back to demo mode if the backend is not available! 🎉
