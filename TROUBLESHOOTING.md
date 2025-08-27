# 🔧 استكشاف الأخطاء وإصلاحها - SCM System

## 🚨 المشاكل الشائعة والحلول

### 1. **JWT Security Vulnerability**
```
Warning: Package 'System.IdentityModel.Tokens.Jwt' 7.0.3 has a known moderate severity vulnerability
```

**الحل:**
```bash
# تشغيل سكريبت الإصلاح
fix-projects.bat

# أو يدوياً
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.2
```

### 2. **Health Checks Error**
```
Error: 'IHealthChecksBuilder' does not contain a definition for 'AddDbContextCheck'
```

**الحل:**
```bash
# إضافة package
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --version 8.0.0
```

### 3. **Null Reference Warning**
```
Warning: Possible null reference argument for parameter 's' in 'byte[] Encoding.GetBytes(string s)'
```

**الحل:**
```csharp
// في Program.cs
var secretKey = jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment";
var key = Encoding.ASCII.GetBytes(secretKey);
```

### 4. **Port Already in Use**
```
Error: Port 7034 is already in use
```

**الحل:**
```bash
# إيقاف جميع الخدمات
stop-all-services.ps1

# أو إيقاف عملية معينة
netstat -ano | findstr :7034
taskkill /PID <PID_NUMBER> /F
```

### 5. **Database Connection Failed**
```
Error: Failed to connect to database
```

**الحل:**
- تأكد من تشغيل SQL Server
- تحقق من connection string في appsettings.json
- تأكد من صحة اسم قاعدة البيانات

### 6. **Build Errors**
```
Error: Build failed
```

**الحل:**
```bash
# تنظيف وإعادة بناء
dotnet clean
dotnet restore
dotnet build

# أو استخدام سكريبت الإصلاح
fix-projects.bat
```

## 🛠️ سكريبتات الإصلاح

### إصلاح جميع المشاريع:
```bash
fix-projects.bat
```

### إصلاح يدوي:
```bash
# 1. تحديث JWT package
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.0.2

# 2. إضافة Health Checks
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --version 8.0.0

# 3. تنظيف وإعادة بناء
dotnet clean
dotnet restore
dotnet build
```

## 📋 قائمة التحقق قبل التشغيل

### ✅ متطلبات النظام:
- [ ] .NET 8.0 مثبت
- [ ] SQL Server يعمل
- [ ] Visual Studio 2022 أو VS Code
- [ ] PowerShell متاح

### ✅ فحص المشاريع:
- [ ] جميع المشاريع تبنى بنجاح
- [ ] لا توجد أخطاء compilation
- [ ] جميع packages محدثة
- [ ] connection strings صحيحة

### ✅ فحص المنافذ:
- [ ] المنافذ 7034, 7133, 7266 متاحة
- [ ] لا توجد خدمات أخرى تستخدم نفس المنافذ
- [ ] Firewall يسمح بالاتصالات

## 🔍 فحص حالة النظام

### فحص المنافذ:
```bash
netstat -an | findstr :7034
netstat -an | findstr :7133
netstat -an | findstr :7266
```

### فحص عمليات dotnet:
```bash
tasklist | findstr dotnet
```

### فحص SQL Server:
```bash
# في SQL Server Management Studio
SELECT @@VERSION
```

## 🚀 تشغيل آمن

### 1. إصلاح المشاكل أولاً:
```bash
fix-projects.bat
```

### 2. تشغيل الخدمات:
```bash
start-simple.bat
```

### 3. اختبار النظام:
- افتح https://localhost:7034/swagger
- اختبر تسجيل الدخول
- تحقق من جميع الخدمات

## 📞 الدعم

### إذا استمرت المشاكل:
1. **تحقق من السجلات** في مجلد logs
2. **راجع ملفات التكوين** appsettings.json
3. **تأكد من إصدارات .NET** و SQL Server
4. **استخدم Visual Studio** لفحص الأخطاء بالتفصيل

### ملفات السجلات:
- `logs/payments-microservice-*.txt`
- `logs/accounting-microservice-*.txt`
- Console output لكل خدمة

## ⚡ حلول سريعة

### إعادة تشغيل كامل:
```bash
# 1. إيقاف جميع الخدمات
stop-all-services.ps1

# 2. إصلاح المشاكل
fix-projects.bat

# 3. تشغيل الخدمات
start-simple.bat
```

### إعادة تشغيل خدمة واحدة:
```bash
# إيقاف الخدمة
Ctrl+C في نافذة الخدمة

# إعادة تشغيل
cd ServiceName
dotnet run
```
