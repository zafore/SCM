@echo off
echo 🚀 تشغيل جميع خدمات نظام SCM
echo ================================================

REM التحقق من وجود PowerShell
powershell -Command "if (Get-Command powershell -ErrorAction SilentlyContinue) { Write-Host '✅ PowerShell متاح' -ForegroundColor Green } else { Write-Host '❌ PowerShell غير متاح' -ForegroundColor Red; exit 1 }"

echo.
echo 📋 بدء تشغيل الخدمات...
echo.

REM تشغيل سكريبت PowerShell
powershell -ExecutionPolicy Bypass -File "start-all-services.ps1"

echo.
echo ✅ تم تشغيل جميع الخدمات!
echo.
echo 🌐 بوابة API: https://localhost:7034
echo 🔐 المصادقة: https://localhost:7133
echo 👨‍💼 الإدارة: https://localhost:7266
echo.
pause
