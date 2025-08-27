# ⚡ تشغيل سريع لنظام SCM

## 🚀 الطريقة السريعة

### Windows
```bash
# تشغيل جميع الخدمات
start-services.bat

# إيقاف جميع الخدمات
stop-services.bat
```

### PowerShell
```powershell
# تشغيل جميع الخدمات
.\start-all-services.ps1

# إيقاف جميع الخدمات
.\stop-all-services.ps1
```

## 📊 الخدمات والمنافذ

| الخدمة | المنفذ | الوصف |
|--------|--------|-------|
| **APIGateWay** | 7034 | بوابة API الرئيسية |
| **IdentityMicroservice** | 7133 | المصادقة |
| **AdminMicroservice** | 7266 | إدارة المستخدمين |
| **CustomerMicroservice** | 7266 | إدارة العملاء |
| **Suppliers.Api** | 7051 | إدارة الموردين |
| **InventoryMicroservice** | 5004 | إدارة المخزون |
| **OrderMicroservice** | 5006 | إدارة الطلبات |
| **Payments.Api** | 5008 | إدارة المدفوعات |
| **Accounting.Api** | 5010 | المحاسبة |

## 🔧 اختبار سريع

### 1. فتح بوابة API
```
https://localhost:7034/swagger
```

### 2. تسجيل الدخول
```http
POST https://localhost:7034/api/identity/auth/login
Content-Type: application/json

{
  "userName": "admin@example.com",
  "password": "password123"
}
```

### 3. استخدام JWT Token
```http
GET https://localhost:7034/api/admin/users
Authorization: Bearer {your-jwt-token}
```

### 4. اختبار الموردين
```http
# عرض جميع الموردين
GET https://localhost:7034/api/suppliers
Authorization: Bearer {your-jwt-token}

# إنشاء مورد جديد
POST https://localhost:7034/api/suppliers
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
  "supplierName": "مورد تجريبي",
  "email": "test@supplier.com",
  "phone": "+966501234567"
}
```

### 5. اختبار العروض
```http
# عرض جميع العروض
GET https://localhost:7034/api/offers
Authorization: Bearer {your-jwt-token}

# عرض البيانات المرجعية
GET https://localhost:7034/api/lookup/countries
Authorization: Bearer {your-jwt-token}
```

## ⚠️ متطلبات

- **.NET 6.0** أو أحدث
- **SQL Server** أو **SQL Server Express**
- **PowerShell** (للسكريبتات)

## 🆘 حل المشاكل

### مشكلة: Port already in use
```bash
# إيقاف جميع الخدمات
.\stop-all-services.ps1

# إعادة التشغيل
.\start-all-services.ps1
```

### مشكلة: Database connection failed
- تأكد من تشغيل SQL Server
- تحقق من connection string في appsettings.json

### مشكلة: JWT token invalid
- تحقق من JWT configuration في APIGateWay
- تأكد من تطابق المفاتيح بين الخدمات
