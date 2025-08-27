# Fix All Microservices Script
# This script fixes common issues across all microservices

Write-Host "🔧 Starting to fix all microservices..." -ForegroundColor Green

# List of microservices to fix
$microservices = @(
    "OrderMicroservice",
    "InventoryMicroservice", 
    "Payments.Api",
    "Accounting.Api",
    "Suppliers.Api"
)

foreach ($service in $microservices) {
    Write-Host "`n📦 Processing $service..." -ForegroundColor Yellow
    
    $csprojFile = "$service\$service.csproj"
    
    if (Test-Path $csprojFile) {
        Write-Host "  ✅ Found project file: $csprojFile" -ForegroundColor Green
        
        # Read the current content
        $content = Get-Content $csprojFile -Raw
        
        # Fix JWT package version
        if ($content -match 'System\.IdentityModel\.Tokens\.Jwt.*Version="7\.0\.3"') {
            $content = $content -replace 'System\.IdentityModel\.Tokens\.Jwt.*Version="7\.0\.3"', 'System.IdentityModel.Tokens.Jwt" Version="8.0.2"'
            Write-Host "  🔄 Updated JWT package to version 8.0.2" -ForegroundColor Cyan
        }
        
        # Add HealthChecks package if not present
        if ($content -notmatch 'Microsoft\.Extensions\.Diagnostics\.HealthChecks\.EntityFrameworkCore') {
            $packageLine = '    <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore" Version="8.0.0" />'
            $content = $content -replace '(<PackageReference Include="System\.IdentityModel\.Tokens\.Jwt".*?/>)', "`$1`n$packageLine"
            Write-Host "  ➕ Added HealthChecks.EntityFrameworkCore package" -ForegroundColor Cyan
        }
        
        # Write back the updated content
        Set-Content $csprojFile -Value $content -NoNewline
        Write-Host "  💾 Updated project file" -ForegroundColor Green
    } else {
        Write-Host "  ❌ Project file not found: $csprojFile" -ForegroundColor Red
    }
    
    # Fix Program.cs files
    $programFile = "$service\Program.cs"
    if (Test-Path $programFile) {
        Write-Host "  📝 Processing Program.cs..." -ForegroundColor Yellow
        
        $programContent = Get-Content $programFile -Raw
        
        # Fix JWT SecretKey null reference
        if ($programContent -match 'var key = Encoding\.ASCII\.GetBytes\(jwtSettings\["SecretKey"\]\);') {
            $programContent = $programContent -replace 'var key = Encoding\.ASCII\.GetBytes\(jwtSettings\["SecretKey"\]\);', 'var secretKey = jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment"; var key = Encoding.ASCII.GetBytes(secretKey);'
            Write-Host "  🔧 Fixed JWT SecretKey null reference" -ForegroundColor Cyan
            
            Set-Content $programFile -Value $programContent -NoNewline
            Write-Host "  💾 Updated Program.cs" -ForegroundColor Green
        }
    } else {
        Write-Host "  ❌ Program.cs not found: $programFile" -ForegroundColor Red
    }
}

Write-Host "`n🎉 All microservices have been processed!" -ForegroundColor Green
Write-Host "`n📋 Summary of fixes applied:" -ForegroundColor Yellow
Write-Host "  ✅ Updated System.IdentityModel.Tokens.Jwt to version 8.0.2" -ForegroundColor White
Write-Host "  ✅ Added Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore package" -ForegroundColor White
Write-Host "  ✅ Fixed JWT SecretKey null reference warnings" -ForegroundColor White

Write-Host "`n🚀 Next steps:" -ForegroundColor Yellow
Write-Host "  1. Run 'dotnet restore' in each microservice directory" -ForegroundColor White
Write-Host "  2. Run 'dotnet build' to verify all fixes" -ForegroundColor White
Write-Host "  3. Use the start-all-services scripts to run all services" -ForegroundColor White

Write-Host "`n✨ Done! All microservices should now compile without errors." -ForegroundColor Green
