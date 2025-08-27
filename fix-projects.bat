@echo off
echo Fixing SCM Projects...
echo.

powershell -ExecutionPolicy Bypass -File "fix-projects.ps1"

echo.
echo Projects fixed!
echo.
pause
