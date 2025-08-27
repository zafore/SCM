# ⚡ دليل البدء السريع - نظام Audit

## 🚀 تشغيل النظام

### 1. تشغيل APIGateWay
```bash
cd APIGateWay
dotnet run
```

### 2. الوصول للخدمة
- **Gateway**: https://localhost:7034
- **Swagger**: https://localhost:7034/swagger

## 🧪 اختبار سريع

### 1. تسجيل الدخول (سيتم تسجيله تلقائياً)
```http
POST https://localhost:7034/api/identity/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Admin123!"
}
```

### 2. عرض إحصائيات النظام
```http
GET https://localhost:7034/api/audit/statistics
Authorization: Bearer {your-jwt-token}
```

### 3. عرض السجلات الحديثة
```http
GET https://localhost:7034/api/audit/recent
Authorization: Bearer {your-jwt-token}
```

### 4. عرض سجلاتي الشخصية
```http
GET https://localhost:7034/api/audit/my-logs
Authorization: Bearer {your-jwt-token}
```

## 📊 العمليات المسجلة تلقائياً

### ✅ **تسجيل الدخول**
- عند تسجيل الدخول عبر `/api/identity/login`
- يتم تسجيل: User ID, IP Address, User Agent, Timestamp

### ✅ **عمليات الموردين**
- عرض الموردين: `GET /api/suppliers`
- إنشاء مورد: `POST /api/suppliers`
- تحديث مورد: `PUT /api/suppliers/{id}`
- حذف مورد: `DELETE /api/suppliers/{id}`

### ✅ **عمليات العروض**
- عرض العروض: `GET /api/offers`
- إنشاء عرض: `POST /api/offers`
- تحديث عرض: `PUT /api/offers/{id}`
- حذف عرض: `DELETE /api/offers/{id}`

### ✅ **البيانات المرجعية**
- عرض البلدان: `GET /api/lookup/countries`
- عرض العملات: `GET /api/lookup/currencies`
- جميع عمليات Lookup

## 🔍 استعلامات مفيدة

### عرض سجلات مورد محدد
```http
GET https://localhost:7034/api/audit/entity/Supplier?entityId=1
Authorization: Bearer {token}
```

### عرض سجلات الدخول
```http
GET https://localhost:7034/api/audit/action/LOGIN
Authorization: Bearer {token}
```

### عرض سجلات الإنشاء
```http
GET https://localhost:7034/api/audit/action/CREATE
Authorization: Bearer {token}
```

### عرض سجلات التحديث
```http
GET https://localhost:7034/api/audit/action/UPDATE
Authorization: Bearer {token}
```

### عرض سجلات الحذف
```http
GET https://localhost:7034/api/audit/action/DELETE
Authorization: Bearer {token}
```

## 🔐 الصلاحيات

### **SuperAdmin**
- ✅ جميع العمليات
- ✅ حذف السجلات

### **Admin**
- ✅ عرض جميع السجلات
- ❌ لا يمكنه حذف السجلات

### **Manager**
- ✅ عرض سجلاته الشخصية
- ✅ عرض سجلات الكيانات

### **User**
- ✅ عرض سجلاته الشخصية فقط

## 📝 مثال كامل

### 1. تسجيل الدخول
```http
POST https://localhost:7034/api/identity/login
Content-Type: application/json

{
  "email": "manager@example.com",
  "password": "Manager123!"
}
```

### 2. إنشاء مورد (سيتم تسجيله تلقائياً)
```http
POST https://localhost:7034/api/suppliers
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "supplierName": "مورد تجريبي",
  "email": "test@supplier.com",
  "phone": "+966501234567"
}
```

### 3. عرض السجلات الحديثة
```http
GET https://localhost:7034/api/audit/recent
Authorization: Bearer {jwt-token}
```

### النتيجة المتوقعة:
```json
{
  "data": [
    {
      "id": 1,
      "userId": "manager123",
      "userName": "Manager User",
      "action": "CREATE",
      "entityType": "Supplier",
      "entityName": "مورد تجريبي",
      "timestamp": "2024-01-15T10:30:00Z",
      "ipAddress": "127.0.0.1",
      "httpMethod": "POST",
      "endpoint": "/api/suppliers"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "timeRange": "Last 24 hours"
}
```

## 🚨 ملاحظات مهمة

1. **التسجيل التلقائي**: جميع العمليات تسجل تلقائياً
2. **لا حاجة لإعداد إضافي**: النظام يعمل فوراً
3. **البيانات في الذاكرة**: للعرض التوضيحي فقط
4. **للإنتاج**: يجب ربطه بقاعدة بيانات

## 🔧 استكشاف الأخطاء

### مشكلة: لا تظهر السجلات
- تأكد من تسجيل الدخول أولاً
- تحقق من JWT token
- تأكد من الصلاحيات

### مشكلة: خطأ 403 Forbidden
- تحقق من دور المستخدم
- تأكد من الصلاحيات المطلوبة

### مشكلة: خطأ 401 Unauthorized
- تحقق من JWT token
- تأكد من صحة Token

## 📞 الدعم

- راجع `AUDIT_SYSTEM.md` للتفاصيل الكاملة
- استخدم `Audit-Test.http` للاختبار
- تحقق من Swagger UI للتوثيق
