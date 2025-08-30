// Test HTTPS Connection
// Run this in your browser console

console.log('🔒 Testing HTTPS Connection');
console.log('==========================');
console.log('');

const testUrls = [
  'https://localhost:5001',
  'https://localhost:3001', 
  'https://localhost:8443',
  'http://localhost:5000',
  'http://localhost:3000'
];

async function testConnection(url) {
  try {
    console.log(`Testing: ${url}`);
    const response = await fetch(`${url}/health`, {
      method: 'GET',
      mode: 'no-cors'
    });
    console.log(`✅ ${url} - Connection successful`);
    return true;
  } catch (error) {
    console.log(`❌ ${url} - Connection failed: ${error.message}`);
    return false;
  }
}

console.log('Testing common backend URLs...');
console.log('');

testUrls.forEach(async (url) => {
  await testConnection(url);
});

console.log('');
console.log('📝 If HTTPS URLs work, update your .env file:');
console.log('VITE_API_BASE_URL=https://localhost:5001');
console.log('');
console.log('🎯 Current configuration:');
console.log('Default URL: https://localhost:5001');
console.log('Environment Variable: VITE_API_BASE_URL');
