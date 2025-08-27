# 🔐 صلاحيات الوصول - Suppliers.Api

## 📋 نظرة عامة

هذا المستند يوضح صلاحيات الوصول لخدمة الموردين عبر API Gateway.

## 🚪 طرق الوصول

### 1. **الوصول المباشر**
- **URL**: `https://localhost:7051`
- **Swagger**: `https://localhost:7051/swagger`
- **مصادقة**: JWT Bearer Token

### 2. **الوصول عبر Gateway**
- **URL**: `https://localhost:7034`
- **Swagger**: `https://localhost:7034/swagger`
- **مصادقة**: JWT Bearer Token + Role-based Authorization

## 🔑 الصلاحيات المطلوبة

### **الموردين (Suppliers)**
| العملية | HTTP Method | الصلاحيات المطلوبة |
|---------|-------------|-------------------|
| عرض جميع الموردين | GET | Manager, Admin, SuperAdmin |
| عرض مورد محدد | GET | Manager, Admin, SuperAdmin |
| إنشاء مورد جديد | POST | Manager, Admin, SuperAdmin |
| تحديث مورد | PUT | Manager, Admin, SuperAdmin |
| حذف مورد | DELETE | Admin, SuperAdmin |
| البحث في الموردين | GET | Manager, Admin, SuperAdmin |
| عرض عروض المورد | GET | Manager, Admin, SuperAdmin |

### **العروض (Offers)**
| العملية | HTTP Method | الصلاحيات المطلوبة |
|---------|-------------|-------------------|
| عرض جميع العروض | GET | Manager, Admin, SuperAdmin |
| عرض عرض محدد | GET | Manager, Admin, SuperAdmin |
| إنشاء عرض جديد | POST | Manager, Admin, SuperAdmin |
| تحديث عرض | PUT | Manager, Admin, SuperAdmin |
| حذف عرض | DELETE | Admin, SuperAdmin |
| عرض عروض مورد | GET | Manager, Admin, SuperAdmin |
| عرض عروض حسب الحالة | GET | Manager, Admin, SuperAdmin |

### **البيانات المرجعية (Lookup)**
| العملية | HTTP Method | الصلاحيات المطلوبة |
|---------|-------------|-------------------|
| عرض البلدان | GET | User, Manager, Admin, SuperAdmin |
| عرض العملات | GET | User, Manager, Admin, SuperAdmin |
| عرض حالات العروض | GET | User, Manager, Admin, SuperAdmin |
| عرض طرق الدفع | GET | User, Manager, Admin, SuperAdmin |
| عرض حالات الدفع | GET | User, Manager, Admin, SuperAdmin |
| عرض شركات الشحن | GET | User, Manager, Admin, SuperAdmin |
| عرض أنواع الشحن | GET | User, Manager, Admin, SuperAdmin |
| عرض أنواع التقسيط | GET | User, Manager, Admin, SuperAdmin |
| عرض المنتجات | GET | User, Manager, Admin, SuperAdmin |

## 🎭 أدوار المستخدمين

### **SuperAdmin**
- ✅ **صلاحيات كاملة** على جميع العمليات
- ✅ **إدارة المستخدمين** والأدوار
- ✅ **حذف البيانات** الحساسة
- ✅ **تعديل الصلاحيات** العامة

### **Admin**
- ✅ **إدارة الموردين** والعروض
- ✅ **إنشاء وتحديث** البيانات
- ✅ **حذف الموردين** والعروض
- ❌ **لا يمكنه** إدارة المستخدمين

### **Manager**
- ✅ **عرض الموردين** والعروض
- ✅ **إنشاء وتحديث** الموردين والعروض
- ❌ **لا يمكنه** حذف البيانات
- ❌ **لا يمكنه** إدارة المستخدمين

### **User**
- ✅ **عرض البيانات المرجعية** فقط
- ❌ **لا يمكنه** الوصول للموردين أو العروض
- ❌ **لا يمكنه** إنشاء أو تعديل البيانات

## 🔒 Gateway Configuration

### **Ocelot Routes**
```json
{
  "UpstreamPathTemplate": "/api/suppliers/{everything}",
  "DownstreamPathTemplate": "/api/suppliers/{everything}",
  "RouteClaimsRequirement": {
    "Role": "Admin,SuperAdmin,Manager"
  }
}
```

### **Permission Config**
```json
{
  "/api/suppliers": {
    "GET": ["Admin", "SuperAdmin", "Manager"],
    "POST": ["Admin", "SuperAdmin", "Manager"],
    "PUT": ["Admin", "SuperAdmin", "Manager"],
    "DELETE": ["Admin", "SuperAdmin"]
  }
}
```

## 🧪 اختبار الصلاحيات

### **1. تسجيل الدخول**
```http
POST https://localhost:7034/api/identity/login
Content-Type: application/json

{
  "email": "manager@example.com",
  "password": "Manager123!"
}
```

### **2. استخدام Token**
```http
GET https://localhost:7034/api/suppliers
Authorization: Bearer {your-jwt-token}
```

### **3. اختبار الصلاحيات**
```http
# هذا سيفشل للمستخدمين العاديين
DELETE https://localhost:7034/api/suppliers/1
Authorization: Bearer {user-token}
```

## 🚨 رسائل الخطأ

### **401 Unauthorized**
```json
{
  "error": "Unauthorized",
  "message": "JWT token is missing or invalid"
}
```

### **403 Forbidden**
```json
{
  "error": "Forbidden",
  "message": "Insufficient permissions. Required role: Manager"
}
```

### **404 Not Found**
```json
{
  "error": "Not Found",
  "message": "Supplier with ID 999 not found"
}
```

## 📝 أمثلة عملية

### **Manager يحاول حذف مورد**
```http
DELETE https://localhost:7034/api/suppliers/1
Authorization: Bearer {manager-token}

# Response: 403 Forbidden
{
  "error": "Forbidden",
  "message": "Insufficient permissions. Required role: Admin"
}
```

### **Admin ينشئ مورد جديد**
```http
POST https://localhost:7034/api/suppliers
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "supplierName": "مورد جديد",
  "email": "new@supplier.com"
}

# Response: 201 Created
```

### **User يحاول الوصول للموردين**
```http
GET https://localhost:7034/api/suppliers
Authorization: Bearer {user-token}

# Response: 403 Forbidden
{
  "error": "Forbidden",
  "message": "Insufficient permissions. Required role: Manager"
}
```

## 🔧 إعداد الصلاحيات

### **1. تحديث Ocelot Configuration**
```bash
# في APIGateWay/ocelot.json
# إضافة routes جديدة للموردين
```

### **2. تحديث Permissions Config**
```bash
# في APIGateWay/permissions-config.json
# إضافة صلاحيات الموردين
```

### **3. إعادة تشغيل Gateway**
```bash
cd APIGateWay
dotnet run
```

## 📊 مراقبة الصلاحيات

### **Logs**
- جميع محاولات الوصول مسجلة
- الأخطاء الأمنية مسجلة
- إحصائيات الاستخدام متاحة

### **Monitoring**
- عدد الطلبات لكل دور
- معدل الأخطاء الأمنية
- أوقات الاستجابة

## ⚠️ ملاحظات أمنية

1. **JWT Tokens** لها مدة صلاحية محدودة
2. **Refresh Tokens** مطلوبة للجلسات الطويلة
3. **HTTPS** مطلوب في الإنتاج
4. **Rate Limiting** مطبق على جميع endpoints
5. **Audit Logs** تسجل جميع العمليات الحساسة
