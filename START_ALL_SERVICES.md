# 🚀 دليل تشغيل جميع خدمات نظام SCM

## 📋 نظرة عامة

هذا الدليل يوضح كيفية تشغيل جميع الخدمات في نظام إدارة سلسلة التوريد (SCM) بالترتيب الصحيح.

## 🏗️ الخدمات المطلوبة

### 1. **قاعدة البيانات** (أولاً)
- **SCMDB**: خدمة قاعدة البيانات

### 2. **خدمات المصادقة والهوية**
- **IdentityMicroservice**: المصادقة وإنشاء JWT tokens
- **AdminMicroservice**: إدارة المستخدمين والأدوار

### 3. **خدمات الأعمال**
- **CustomerMicroservice**: إدارة العملاء
- **Suppliers.Api**: إدارة الموردين
- **InventoryMicroservice**: إدارة المخزون
- **OrderMicroservice**: إدارة الطلبات
- **Payments.Api**: إدارة المدفوعات
- **Accounting.Api**: المحاسبة

### 4. **بوابة API** (أخيراً)
- **APIGateWay**: بوابة API الرئيسية مع Ocelot

## 🔧 طرق التشغيل

### الطريقة الأولى: تشغيل يدوي (Terminal/Command Prompt)

#### 1. تشغيل قاعدة البيانات
```bash
# Terminal 1
cd SCMDB
dotnet run
```

#### 2. تشغيل خدمات المصادقة
```bash
# Terminal 2
cd IdentityMicroservice
dotnet run

# Terminal 3
cd AdminMicroservice
dotnet run
```

#### 3. تشغيل خدمات الأعمال
```bash
# Terminal 4
cd CustomerMicroservice
dotnet run

# Terminal 5
cd Suppliers.Api
dotnet run

# Terminal 6
cd InventoryMicroservice
dotnet run

# Terminal 7
cd OrderMicroservice
dotnet run

# Terminal 8
cd Payments.Api
dotnet run

# Terminal 9
cd Accounting.Api
dotnet run
```

#### 4. تشغيل بوابة API
```bash
# Terminal 10
cd APIGateWay
dotnet run
```

### الطريقة الثانية: استخدام Visual Studio

#### 1. فتح Solution
```bash
# فتح ملف الحل
SCM.sln
```

#### 2. تشغيل متعدد المشاريع
1. **Right-click** على Solution
2. **Properties**
3. **Multiple startup projects**
4. **Set action** لكل مشروع إلى **Start**

#### 3. ترتيب التشغيل
```
1. SCMDB (Start)
2. IdentityMicroservice (Start)
3. AdminMicroservice (Start)
4. CustomerMicroservice (Start)
5. Suppliers.Api (Start)
6. InventoryMicroservice (Start)
7. OrderMicroservice (Start)
8. Payments.Api (Start)
9. Accounting.Api (Start)
10. APIGateWay (Start)
```

### الطريقة الثالثة: استخدام Docker Compose (مستقبلاً)

```yaml
# docker-compose.yml
version: '3.8'
services:
  scmdb:
    build: ./SCMDB
    ports:
      - "1433:1433"
  
  identity:
    build: ./IdentityMicroservice
    ports:
      - "7133:7133"
    depends_on:
      - scmdb
  
  admin:
    build: ./AdminMicroservice
    ports:
      - "7266:7266"
    depends_on:
      - scmdb
  
  gateway:
    build: ./APIGateWay
    ports:
      - "7034:7034"
    depends_on:
      - identity
      - admin
```

## 📊 منافذ الخدمات

| الخدمة | HTTP Port | HTTPS Port | الوصف |
|--------|-----------|------------|-------|
| **SCMDB** | - | - | قاعدة البيانات |
| **IdentityMicroservice** | 5213 | 7133 | المصادقة |
| **AdminMicroservice** | 5076 | 7266 | إدارة المستخدمين |
| **CustomerMicroservice** | 5076 | 7266 | إدارة العملاء |
| **Suppliers.Api** | 5194 | 7051 | إدارة الموردين |
| **InventoryMicroservice** | 5003 | 5004 | إدارة المخزون |
| **OrderMicroservice** | 5005 | 5006 | إدارة الطلبات |
| **Payments.Api** | 5007 | 5008 | إدارة المدفوعات |
| **Accounting.Api** | 5009 | 5010 | المحاسبة |
| **APIGateWay** | 5197 | 7034 | بوابة API |

## 🔍 التحقق من التشغيل

### 1. فحص حالة الخدمات
```bash
# فحص المنافذ المستخدمة
netstat -an | findstr :7034
netstat -an | findstr :7133
netstat -an | findstr :7266
```

### 2. اختبار الخدمات
```bash
# اختبار بوابة API
curl https://localhost:7034/swagger

# اختبار المصادقة
curl https://localhost:7133/swagger

# اختبار الإدارة
curl https://localhost:7266/swagger
```

### 3. اختبار المصادقة
```http
POST https://localhost:7034/api/identity/auth/login
Content-Type: application/json

{
  "userName": "admin@example.com",
  "password": "password123"
}
```

## ⚠️ نصائح مهمة

### 1. ترتيب التشغيل
- **ابدأ بقاعدة البيانات** أولاً
- **ثم خدمات المصادقة**
- **ثم خدمات الأعمال**
- **أخيراً بوابة API**

### 2. متطلبات النظام
- **.NET 6.0** أو أحدث
- **SQL Server** أو **SQL Server Express**
- **Visual Studio 2022** أو **VS Code**

### 3. حل المشاكل الشائعة
```bash
# تنظيف الحل
dotnet clean
dotnet restore
dotnet build

# إعادة تشغيل خدمة
dotnet run --urls "https://localhost:7034"
```

### 4. متغيرات البيئة
```bash
# تطوير
ASPNETCORE_ENVIRONMENT=Development

# إنتاج
ASPNETCORE_ENVIRONMENT=Production
```

## 🚀 تشغيل سريع

### سكريبت PowerShell (Windows)
```powershell
# start-all-services.ps1
Write-Host "Starting SCM Services..." -ForegroundColor Green

# Start Database
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd SCMDB; dotnet run"

# Wait 5 seconds
Start-Sleep -Seconds 5

# Start Identity Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd IdentityMicroservice; dotnet run"

# Start Admin Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd AdminMicroservice; dotnet run"

# Start Customer Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd CustomerMicroservice; dotnet run"

# Start Suppliers Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Suppliers.Api; dotnet run"

# Start Inventory Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd InventoryMicroservice; dotnet run"

# Start Order Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd OrderMicroservice; dotnet run"

# Start Payments Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Payments.Api; dotnet run"

# Start Accounting Service
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Accounting.Api; dotnet run"

# Wait 10 seconds
Start-Sleep -Seconds 10

# Start API Gateway
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd APIGateWay; dotnet run"

Write-Host "All services started!" -ForegroundColor Green
```

### سكريبت Bash (Linux/Mac)
```bash
#!/bin/bash
# start-all-services.sh

echo "Starting SCM Services..."

# Start Database
cd SCMDB && dotnet run &
sleep 5

# Start Identity Service
cd ../IdentityMicroservice && dotnet run &
sleep 2

# Start Admin Service
cd ../AdminMicroservice && dotnet run &
sleep 2

# Start Customer Service
cd ../CustomerMicroservice && dotnet run &
sleep 2

# Start Suppliers Service
cd ../Suppliers.Api && dotnet run &
sleep 2

# Start Inventory Service
cd ../InventoryMicroservice && dotnet run &
sleep 2

# Start Order Service
cd ../OrderMicroservice && dotnet run &
sleep 2

# Start Payments Service
cd ../Payments.Api && dotnet run &
sleep 2

# Start Accounting Service
cd ../Accounting.Api && dotnet run &
sleep 2

# Start API Gateway
cd ../APIGateWay && dotnet run &

echo "All services started!"
```

## 📝 ملاحظات مهمة

1. **تأكد من تشغيل SQL Server** قبل بدء الخدمات
2. **تحقق من المنافذ** المتاحة
3. **راجع ملفات التكوين** قبل التشغيل
4. **راقب السجلات** للتأكد من التشغيل الصحيح
5. **استخدم HTTPS** في الإنتاج

## 🔧 استكشاف الأخطاء

### مشاكل شائعة:
1. **Port already in use**: غير المنفذ في launchSettings.json
2. **Database connection failed**: تحقق من connection string
3. **JWT token invalid**: تحقق من JWT configuration
4. **Service not found**: تحقق من ocelot.json routing

### سجلات مفيدة:
- **APIGateWay**: logs في console
- **IdentityMicroservice**: logs في console
- **AdminMicroservice**: logs في console
