@echo off
echo 🛑 إيقاف جميع خدمات نظام SCM
echo ================================================

echo.
echo 📋 إيقاف الخدمات...
echo.

REM تشغيل سكريبت PowerShell
powershell -ExecutionPolicy Bypass -File "stop-all-services.ps1"

echo.
echo ✅ تم إيقاف جميع الخدمات!
echo.
pause
