// Debug Token Script
// Run this in the browser console to check if token is being sent with requests

console.log('🔍 Debug Token Script Loaded');

// Override fetch to log all requests
const originalFetch = window.fetch;
window.fetch = function(...args) {
  const [url, options = {}] = args;
  
  console.log('🌐 Fetch Request:', {
    url: url,
    method: options.method || 'GET',
    headers: options.headers,
    hasAuth: options.headers?.Authorization ? 'YES' : 'NO',
    authValue: options.headers?.Authorization ? options.headers.Authorization.substring(0, 20) + '...' : 'NONE'
  });
  
  return originalFetch.apply(this, args);
};

// Override axios to log all requests
if (window.axios) {
  const originalAxios = window.axios;
  window.axios.interceptors.request.use(
    function (config) {
      console.log('🚀 Axios Request:', {
        url: config.url,
        method: config.method?.toUpperCase(),
        headers: config.headers,
        hasAuth: config.headers?.Authorization ? 'YES' : 'NO',
        authValue: config.headers?.Authorization ? config.headers.Authorization.substring(0, 20) + '...' : 'NONE'
      });
      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
}

// Check current token
function checkCurrentToken() {
  const token = localStorage.getItem('token');
  const user = localStorage.getItem('user');
  
  console.log('🔑 Current Token Status:');
  console.log('- Token exists:', !!token);
  console.log('- User exists:', !!user);
  
  if (token) {
    console.log('- Token preview:', token.substring(0, 50) + '...');
    console.log('- Token length:', token.length);
    
    try {
      const decoded = JSON.parse(atob(token.split('.')[1]));
      console.log('- Token payload:', decoded);
      console.log('- User email:', decoded.email);
      console.log('- User role:', decoded.role);
      console.log('- Token expired:', decoded.exp * 1000 < Date.now());
    } catch (e) {
      console.log('- Error decoding token:', e.message);
    }
  }
  
  return { token, user };
}

// Test API call manually
async function testApiCall() {
  const token = localStorage.getItem('token');
  
  if (!token) {
    console.log('❌ No token available');
    return;
  }
  
  console.log('🧪 Testing API call with token...');
  
  try {
    const response = await fetch('https://localhost:7034/api/suppliers?page=1&pageSize=10', {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
    
    console.log('📊 Response:', {
      status: response.status,
      statusText: response.statusText,
      ok: response.ok
    });
    
    if (response.ok) {
      const data = await response.json();
      console.log('✅ Success! Data received:', data);
    } else {
      const error = await response.text();
      console.log('❌ Error response:', error);
    }
  } catch (error) {
    console.log('❌ Network error:', error.message);
  }
}

// Test login and then API call
async function testFullFlow() {
  console.log('🚀 Testing Full Authentication Flow...');
  
  // Step 1: Login
  console.log('Step 1: Logging in...');
  try {
    const response = await fetch('https://localhost:7133/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        Email: 'admin@test.com',
        Password: 'Admin123!'
      })
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify(data.user || data));
      
      console.log('✅ Login successful!');
      console.log('- Token stored:', !!data.token);
    } else {
      const error = await response.text();
      console.log('❌ Login failed:', error);
      return;
    }
  } catch (error) {
    console.log('❌ Login error:', error.message);
    return;
  }
  
  // Step 2: Check token
  console.log('Step 2: Checking token...');
  checkCurrentToken();
  
  // Step 3: Test API call
  console.log('Step 3: Testing API call...');
  await testApiCall();
}

// Export functions
window.tokenDebug = {
  checkCurrentToken,
  testApiCall,
  testFullFlow
};

console.log('📝 Available debug functions:');
console.log('- tokenDebug.checkCurrentToken()');
console.log('- tokenDebug.testApiCall()');
console.log('- tokenDebug.testFullFlow()');
console.log('');
console.log('💡 Run tokenDebug.testFullFlow() to test the complete flow');
console.log('💡 All fetch/axios requests will now be logged automatically');
