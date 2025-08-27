@echo off
echo Starting SCM Services...
echo.

powershell -ExecutionPolicy Bypass -File "start-services-simple.ps1"

echo.
echo Services started!
echo API Gateway: https://localhost:7034
echo.
pause
