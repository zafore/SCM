@echo off
echo 🔧 Starting to fix all microservices...
echo.

powershell -ExecutionPolicy Bypass -File "fix-all-microservices.ps1"

echo.
echo ✅ Script completed!
pause
