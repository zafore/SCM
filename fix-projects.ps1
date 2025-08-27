# Fix Common Issues in SCM Projects
Write-Host "Fixing common issues in SCM projects..." -ForegroundColor Green

# List of projects that need fixing
$projects = @(
    "Payments.Api",
    "Accounting.Api",
    "Suppliers.Api",
    "InventoryMicroservice",
    "OrderMicroservice"
)

foreach ($project in $projects) {
    Write-Host "`nFixing $project..." -ForegroundColor Yellow
    
    $csprojPath = "$project\$project.csproj"
    
    if (Test-Path $csprojPath) {
        # Read the project file
        $content = Get-Content $csprojPath -Raw
        
        # Fix JWT version
        $content = $content -replace 'System\.IdentityModel\.Tokens\.Jwt.*Version="7\.0\.3"', 'System.IdentityModel.Tokens.Jwt" Version="8.0.2"'
        
        # Add Health Checks package if not present
        if ($content -notmatch "Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore") {
            $content = $content -replace '(\s+</ItemGroup>)', "    <PackageReference Include=`"Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore`" Version=`"8.0.0`" />`n$1"
        }
        
        # Write back the fixed content
        Set-Content $csprojPath $content -NoNewline
        Write-Host "Fixed $csprojPath" -ForegroundColor Green
    } else {
        Write-Host "Project file not found: $csprojPath" -ForegroundColor Red
    }
}

Write-Host "`nRestoring packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "`nBuilding solution..." -ForegroundColor Yellow
dotnet build

Write-Host "`nAll fixes applied!" -ForegroundColor Green
