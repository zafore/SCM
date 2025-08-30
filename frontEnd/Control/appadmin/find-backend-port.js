// Script to help find your backend port
// Run this in your browser console or Node.js

console.log('🔍 Backend Port Finder');
console.log('=====================');
console.log('');
console.log('Common backend ports to check:');
console.log('• .NET Core: http://localhost:5000');
console.log('• .NET Core (HTTPS): https://localhost:5001');
console.log('• Express.js: http://localhost:3000');
console.log('• Flask: http://localhost:5000');
console.log('• Django: http://localhost:8000');
console.log('• Spring Boot: http://localhost:8080');
console.log('');

// Function to test if a port is available
async function testPort(port) {
  try {
    const response = await fetch(`http://localhost:${port}/health`, {
      method: 'GET',
      mode: 'no-cors'
    });
    return true;
  } catch (error) {
    return false;
  }
}

// Test common ports
const commonPorts = [3000, 5000, 5001, 8000, 8080, 3001, 4000];

console.log('Testing common ports...');
commonPorts.forEach(async (port) => {
  const isAvailable = await testPort(port);
  if (isAvailable) {
    console.log(`✅ Port ${port} might be available`);
  }
});

console.log('');
console.log('📝 To configure your frontend:');
console.log('1. Create a .env file in your project root');
console.log('2. Add: VITE_API_BASE_URL=http://localhost:YOUR_PORT');
console.log('3. Restart your frontend (npm start)');
console.log('');
console.log('🎯 If you know your backend port, update:');
console.log('src/api/config/endpoints.js');
console.log('Change the default URL to your backend port');
