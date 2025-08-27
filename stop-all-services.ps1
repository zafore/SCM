# 🛑 سكريبت إيقاف جميع خدمات نظام SCM
# PowerShell Script to Stop All SCM Services

Write-Host "🛑 إيقاف جميع خدمات نظام SCM..." -ForegroundColor Red
Write-Host "================================================" -ForegroundColor Cyan

# إيقاف عمليات dotnet
Write-Host "`n🔍 البحث عن عمليات dotnet..." -ForegroundColor Yellow

$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue

if ($dotnetProcesses) {
    Write-Host "✅ تم العثور على $($dotnetProcesses.Count) عملية dotnet" -ForegroundColor Green
    
    Write-Host "`n📋 قائمة العمليات:" -ForegroundColor Cyan
    foreach ($process in $dotnetProcesses) {
        Write-Host "- Process ID: $($process.Id), CPU: $($process.CPU), Memory: $([math]::Round($process.WorkingSet64/1MB, 2)) MB" -ForegroundColor White
    }
    
    Write-Host "`n⚠️ هل تريد إيقاف جميع عمليات dotnet؟ (Y/N)" -ForegroundColor Yellow
    $confirmation = Read-Host
    
    if ($confirmation -eq 'Y' -or $confirmation -eq 'y') {
        Write-Host "`n🛑 إيقاف عمليات dotnet..." -ForegroundColor Red
        
        foreach ($process in $dotnetProcesses) {
            try {
                Stop-Process -Id $process.Id -Force
                Write-Host "✅ تم إيقاف العملية $($process.Id)" -ForegroundColor Green
            } catch {
                Write-Host "❌ فشل في إيقاف العملية $($process.Id): $($_.Exception.Message)" -ForegroundColor Red
            }
        }
        
        Write-Host "`n✅ تم إيقاف جميع عمليات dotnet" -ForegroundColor Green
    } else {
        Write-Host "`n⏸️ تم إلغاء العملية" -ForegroundColor Yellow
    }
} else {
    Write-Host "ℹ️ لا توجد عمليات dotnet قيد التشغيل" -ForegroundColor Blue
}

# إيقاف عمليات PowerShell المفتوحة
Write-Host "`n🔍 البحث عن نوافذ PowerShell المفتوحة..." -ForegroundColor Yellow

$powershellWindows = Get-Process -Name "powershell" -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowTitle -like "*dotnet run*" -or $_.MainWindowTitle -like "*SCM*" }

if ($powershellWindows) {
    Write-Host "✅ تم العثور على $($powershellWindows.Count) نافذة PowerShell" -ForegroundColor Green
    
    Write-Host "`n⚠️ هل تريد إغلاق نوافذ PowerShell المفتوحة؟ (Y/N)" -ForegroundColor Yellow
    $confirmation = Read-Host
    
    if ($confirmation -eq 'Y' -or $confirmation -eq 'y') {
        Write-Host "`n🛑 إغلاق نوافذ PowerShell..." -ForegroundColor Red
        
        foreach ($window in $powershellWindows) {
            try {
                $window.CloseMainWindow()
                Write-Host "✅ تم إغلاق النافذة $($window.Id)" -ForegroundColor Green
            } catch {
                Write-Host "❌ فشل في إغلاق النافذة $($window.Id): $($_.Exception.Message)" -ForegroundColor Red
            }
        }
        
        Write-Host "`n✅ تم إغلاق نوافذ PowerShell" -ForegroundColor Green
    } else {
        Write-Host "`n⏸️ تم إلغاء العملية" -ForegroundColor Yellow
    }
} else {
    Write-Host "ℹ️ لا توجد نوافذ PowerShell مفتوحة" -ForegroundColor Blue
}

# فحص المنافذ المستخدمة
Write-Host "`n🔍 فحص المنافذ المستخدمة..." -ForegroundColor Yellow

$ports = @(7034, 7133, 7266, 7051, 5004, 5006, 5008, 5010)
$usedPorts = @()

foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        $usedPorts += $port
        Write-Host "⚠️ المنفذ $port لا يزال مستخدماً" -ForegroundColor Yellow
    } else {
        Write-Host "✅ المنفذ $port متاح" -ForegroundColor Green
    }
}

if ($usedPorts.Count -gt 0) {
    Write-Host "`n⚠️ بعض المنافذ لا تزال مستخدمة. قد تحتاج لإعادة تشغيل النظام." -ForegroundColor Yellow
} else {
    Write-Host "`n✅ جميع المنافذ متاحة" -ForegroundColor Green
}

Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host "🎉 تم إيقاف جميع خدمات نظام SCM!" -ForegroundColor Green
