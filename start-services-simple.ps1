# Simple SCM Services Startup Script
Write-Host "Starting SCM Services..." -ForegroundColor Green

# Start Database
Write-Host "Starting Database..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd SCMDB; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# Start Identity Service
Write-Host "Starting Identity Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd IdentityMicroservice; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# Start Admin Service
Write-Host "Starting Admin Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd AdminMicroservice; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# Start Customer Service
Write-Host "Starting Customer Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd CustomerMicroservice; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# Start Suppliers Service
Write-Host "Starting Suppliers Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Suppliers.Api; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# Start Inventory Service
Write-Host "Starting Inventory Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd InventoryMicroservice; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# Start Order Service
Write-Host "Starting Order Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd OrderMicroservice; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# Start Payments Service
Write-Host "Starting Payments Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Payments.Api; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 2

# Start Accounting Service
Write-Host "Starting Accounting Service..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Accounting.Api; dotnet run" -WindowStyle Normal
Start-Sleep -Seconds 3

# Start API Gateway
Write-Host "Starting API Gateway..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd APIGateWay; dotnet run" -WindowStyle Normal

Write-Host "All services started!" -ForegroundColor Green
Write-Host "API Gateway: https://localhost:7034" -ForegroundColor White
Write-Host "Wait 30 seconds for all services to be ready" -ForegroundColor Yellow
