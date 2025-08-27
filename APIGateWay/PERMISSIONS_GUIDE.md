# 🔐 دليل إدارة الصلاحيات في APIGateWay

## 📋 نظرة عامة

هذا النظام يوفر إدارة شاملة للصلاحيات في APIGateWay باستخدام Ocelot مع دمج AdminMicroservice لإدارة المستخدمين والأدوار.

## 🏗️ البنية

### 1. **APIGateWay** - بوابة API الرئيسية
- **Ocelot Configuration**: `ocelot.json` - تكوين المسارات والصلاحيات
- **Permissions Config**: `permissions-config.json` - تكوين الصلاحيات الديناميكية
- **Permission Service**: خدمة التحقق من الصلاحيات

### 2. **AdminMicroservice** - إدارة المستخدمين والأدوار
- **UserManagementController**: إدارة المستخدمين
- **RoleManagementController**: إدارة الأدوار
- **PermissionManagementController**: إدارة صلاحيات المسارات

### 3. **IdentityMicroservice** - المصادقة
- **AuthController**: تسجيل الدخول وإنشاء JWT tokens
- **User/Role Entities**: نماذج البيانات

## 🎯 مستويات الصلاحيات

### الأدوار المتاحة:
1. **SuperAdmin**: صلاحيات كاملة
2. **Admin**: إدارة المستخدمين والأدوار
3. **Manager**: إدارة العمليات التجارية
4. **User**: صلاحيات محدودة

### هرمية الصلاحيات:
```
SuperAdmin → Admin → Manager → User
```

## 🔧 كيفية الاستخدام

### 1. تسجيل الدخول
```http
POST /api/identity/auth/login
Content-Type: application/json

{
  "userName": "admin@example.com",
  "password": "password123"
}
```

### 2. إدارة المستخدمين (Admin/SuperAdmin فقط)
```http
# إنشاء مستخدم جديد
POST /api/admin/users
Authorization: Bearer {token}
Content-Type: application/json

{
  "userName": "newuser@example.com",
  "password": "password123",
  "fullName": "New User",
  "email": "newuser@example.com",
  "phone": "1234567890",
  "roleIds": [2, 3]
}

# تعيين دور للمستخدم
POST /api/admin/users/{userId}/roles
Authorization: Bearer {token}
Content-Type: application/json

{
  "roleId": 2
}
```

### 3. إدارة الأدوار (SuperAdmin فقط)
```http
# إنشاء دور جديد
POST /api/admin/roles
Authorization: Bearer {token}
Content-Type: application/json

{
  "roleName": "CustomRole",
  "roleTypeId": 1
}
```

### 4. إدارة صلاحيات المسارات (SuperAdmin فقط)
```http
# تعيين صلاحيات مسار لدور
POST /api/admin/permissions/routes/{routeId}/roles
Authorization: Bearer {token}
Content-Type: application/json

{
  "roleId": 2,
  "allowedMethods": ["GET", "POST"]
}
```

## 📊 صلاحيات المسارات

| المسار | GET | POST | PUT | DELETE |
|--------|-----|------|-----|--------|
| `/api/admin/users` | Admin+ | Admin+ | Admin+ | SuperAdmin |
| `/api/admin/roles` | Admin+ | SuperAdmin | SuperAdmin | SuperAdmin |
| `/suppliers` | All | Manager+ | Manager+ | Admin+ |
| `/api/inventory` | All | Manager+ | Manager+ | Admin+ |
| `/api/orders` | All | All | Manager+ | Admin+ |
| `/api/payments` | Manager+ | Manager+ | Manager+ | Admin+ |
| `/api/accounting` | Manager+ | Manager+ | Manager+ | Admin+ |

## ⚙️ التكوين

### 1. تحديث ocelot.json
```json
{
  "UpstreamPathTemplate": "/api/admin/{everything}",
  "AuthenticationOptions": {
    "AuthenticationProviderKey": "Bearer"
  },
  "RouteClaimsRequirement": {
    "Role": "Admin,SuperAdmin"
  }
}
```

### 2. تحديث permissions-config.json
```json
{
  "RoutePermissions": {
    "/api/admin/users": {
      "GET": ["Admin", "SuperAdmin"],
      "POST": ["Admin", "SuperAdmin"]
    }
  }
}
```

## 🔄 التحديث الديناميكي

يمكن تحديث الصلاحيات دون إعادة تشغيل الخدمة:

1. **تحديث permissions-config.json**
2. **استخدام AdminMicroservice APIs**
3. **إعادة تحميل التكوين تلقائياً**

## 🛡️ الأمان

### أفضل الممارسات:
1. **استخدام HTTPS** في الإنتاج
2. **تحديث JWT Secret Key** بانتظام
3. **مراجعة الصلاحيات** دورياً
4. **تسجيل العمليات** الحساسة
5. **تحديد صلاحيات محدودة** للمستخدمين

### نصائح الأمان:
- لا تشارك JWT tokens
- استخدم كلمات مرور قوية
- فعّل تسجيل العمليات
- راجع الصلاحيات بانتظام

## 🚀 النشر

### متطلبات النشر:
1. **قاعدة البيانات**: SQL Server مع جداول المستخدمين والأدوار
2. **JWT Configuration**: تكوين صحيح للمفاتيح
3. **Network**: اتصال بين الخدمات
4. **SSL Certificates**: للاتصالات الآمنة

### متغيرات البيئة:
```bash
JWT_SECRET_KEY=your-secret-key
JWT_ISSUER=your-issuer
JWT_AUDIENCE=your-audience
DB_CONNECTION_STRING=your-connection-string
```

## 📝 ملاحظات مهمة

1. **AdminMicroservice** يجب أن يكون متاحاً لإدارة المستخدمين
2. **IdentityMicroservice** مسؤول عن المصادقة فقط
3. **APIGateWay** يتحقق من الصلاحيات قبل التوجيه
4. **Ocelot** يدعم التحقق من Claims في JWT tokens

## 🔧 استكشاف الأخطاء

### مشاكل شائعة:
1. **401 Unauthorized**: تحقق من JWT token
2. **403 Forbidden**: تحقق من الصلاحيات
3. **500 Internal Server Error**: تحقق من تكوين قاعدة البيانات

### سجلات مفيدة:
- APIGateWay logs
- AdminMicroservice logs
- IdentityMicroservice logs
