# Start All SCM Services PowerShell Script
# PowerShell Script to Start All SCM Services

Write-Host "Starting All SCM Services..." -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan

# Check .NET availability
try {
    $dotnetVersion = dotnet --version
    Write-Host "OK .NET Version: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "ERROR .NET not installed or not available in PATH" -ForegroundColor Red
    exit 1
}

# Check SQL Server availability
Write-Host "Checking SQL Server..." -ForegroundColor Yellow
try {
    $sqlServer = Get-Service -Name "MSSQL*" -ErrorAction SilentlyContinue
    if ($sqlServer) {
        Write-Host "OK SQL Server available" -ForegroundColor Green
    } else {
        Write-Host "WARNING Check SQL Server is running" -ForegroundColor Yellow
    }
} catch {
    Write-Host "WARNING Check SQL Server is running" -ForegroundColor Yellow
}

Write-Host "`nStarting services in correct order..." -ForegroundColor Cyan

# 1. Start Database
Write-Host "`n1. Starting Database (SCMDB)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd SCMDB; Write-Host 'SCMDB - Database' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# 2. Start Identity Service
Write-Host "2. Starting Identity Service (IdentityMicroservice)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd IdentityMicroservice; Write-Host 'IdentityMicroservice - Authentication (Port: 7133)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# 3. Start Admin Service
Write-Host "3. Starting Admin Service (AdminMicroservice)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd AdminMicroservice; Write-Host 'AdminMicroservice - User Management (Port: 7266)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# 4. Start Customer Service
Write-Host "4. Starting Customer Service (CustomerMicroservice)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd CustomerMicroservice; Write-Host 'CustomerMicroservice - Customer Management (Port: 7266)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# 5. Start Suppliers Service
Write-Host "5. Starting Suppliers Service (Suppliers.Api)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Suppliers.Api; Write-Host 'Suppliers.Api - Supplier Management (Port: 7051)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# 6. Start Inventory Service
Write-Host "6. Starting Inventory Service (InventoryMicroservice)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd InventoryMicroservice; Write-Host 'InventoryMicroservice - Inventory Management (Port: 5004)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# 7. Start Order Service
Write-Host "7. Starting Order Service (OrderMicroservice)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd OrderMicroservice; Write-Host 'OrderMicroservice - Order Management (Port: 5006)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# 8. Start Payments Service
Write-Host "8. Starting Payments Service (Payments.Api)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Payments.Api; Write-Host 'Payments.Api - Payment Management (Port: 5008)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# 9. Start Accounting Service
Write-Host "9. Starting Accounting Service (Accounting.Api)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Accounting.Api; Write-Host 'Accounting.Api - Accounting (Port: 5010)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# 10. Start API Gateway (Last)
Write-Host "10. Starting API Gateway (APIGateWay)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd APIGateWay; Write-Host 'APIGateWay - Main API Gateway (Port: 7034)' -ForegroundColor Magenta; dotnet run" -WindowStyle Normal

Write-Host "`nAll services started successfully!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan

Write-Host "`nService Information:" -ForegroundColor Cyan
Write-Host "Main API Gateway: https://localhost:7034" -ForegroundColor White
Write-Host "Authentication Service: https://localhost:7133" -ForegroundColor White
Write-Host "Admin Service: https://localhost:7266" -ForegroundColor White
Write-Host "Customer Service: https://localhost:7266" -ForegroundColor White
Write-Host "Suppliers Service: https://localhost:7051" -ForegroundColor White
Write-Host "Inventory Service: https://localhost:5004" -ForegroundColor White
Write-Host "Order Service: https://localhost:5006" -ForegroundColor White
Write-Host "Payments Service: https://localhost:5008" -ForegroundColor White
Write-Host "Accounting Service: https://localhost:5010" -ForegroundColor White

Write-Host "`nTesting Services:" -ForegroundColor Cyan
Write-Host "1. Open https://localhost:7034/swagger to test API Gateway" -ForegroundColor White
Write-Host "2. Use POST /api/identity/auth/login for authentication" -ForegroundColor White
Write-Host "3. Use JWT token to access protected services" -ForegroundColor White

Write-Host "`nImportant Notes:" -ForegroundColor Yellow
Write-Host "- Make sure SQL Server is running" -ForegroundColor White
Write-Host "- Wait 30 seconds for all services to start" -ForegroundColor White
Write-Host "- Use Ctrl+C to stop any service" -ForegroundColor White

Write-Host "`nSCM System started successfully!" -ForegroundColor Green
