# 🏭 Suppliers.Api - خدمة إدارة الموردين

## 📋 نظرة عامة

Suppliers.Api هي خدمة ميكروسيرفس متخصصة في إدارة الموردين والعروض في نظام إدارة سلسلة التوريد (SCM).

## 🚀 الميزات

### إدارة الموردين
- ✅ إنشاء موردين جدد
- ✅ عرض جميع الموردين
- ✅ تحديث معلومات الموردين
- ✅ حذف الموردين (Soft Delete)
- ✅ البحث في الموردين
- ✅ عرض عروض المورد

### إدارة العروض
- ✅ إنشاء عروض جديدة
- ✅ عرض جميع العروض
- ✅ تحديث العروض
- ✅ حذف العروض
- ✅ عرض العروض حسب المورد
- ✅ عرض العروض حسب الحالة

### البيانات المرجعية
- ✅ البلدان
- ✅ العملات
- ✅ حالات العروض
- ✅ طرق الدفع
- ✅ حالات الدفع
- ✅ شركات الشحن
- ✅ أنواع الشحن
- ✅ أنواع التقسيط
- ✅ المنتجات

## 🔗 API Endpoints

### الموردين
```
GET    /api/suppliers              - عرض جميع الموردين
GET    /api/suppliers/{id}         - عرض مورد محدد
POST   /api/suppliers              - إنشاء مورد جديد
PUT    /api/suppliers/{id}         - تحديث مورد
DELETE /api/suppliers/{id}         - حذف مورد
GET    /api/suppliers/search       - البحث في الموردين
GET    /api/suppliers/{id}/offers  - عرض عروض المورد
```

### العروض
```
GET    /api/offers                 - عرض جميع العروض
GET    /api/offers/{id}            - عرض عرض محدد
POST   /api/offers                 - إنشاء عرض جديد
PUT    /api/offers/{id}            - تحديث عرض
DELETE /api/offers/{id}            - حذف عرض
GET    /api/offers/supplier/{id}   - عرض عروض مورد محدد
GET    /api/offers/status/{id}     - عرض عروض حسب الحالة
```

### البيانات المرجعية
```
GET    /api/lookup/countries           - البلدان
GET    /api/lookup/currencies          - العملات
GET    /api/lookup/offer-statuses      - حالات العروض
GET    /api/lookup/payment-methods     - طرق الدفع
GET    /api/lookup/payment-states      - حالات الدفع
GET    /api/lookup/carriers            - شركات الشحن
GET    /api/lookup/shipment-types      - أنواع الشحن
GET    /api/lookup/installments-types  - أنواع التقسيط
GET    /api/lookup/items               - المنتجات
```

## 🗄️ قاعدة البيانات

### الجداول الرئيسية
- **Suppliers** - الموردين
- **Offers** - العروض
- **OfferItems** - عناصر العروض
- **OfferContracts** - عقود العروض
- **OfferShippingCosts** - تكاليف الشحن
- **SupplierContacts** - جهات اتصال الموردين

### الجداول المرجعية
- **Countries** - البلدان
- **Currencies** - العملات
- **OfferStatuses** - حالات العروض
- **PaymentMethods** - طرق الدفع
- **PaymentStates** - حالات الدفع
- **Carriers** - شركات الشحن
- **ShipmentTypes** - أنواع الشحن
- **InstallmentsTypes** - أنواع التقسيط
- **Items** - المنتجات

## 🔧 التكوين

### appsettings.json
```json
{
  "ConnectionStrings": {
    "ConUser": "Server=localhost;Database=SCMDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "http://localhost:7133",
    "Audience": "http://localhost:7034"
  }
}
```

## 🚀 التشغيل

### متطلبات
- .NET 8.0
- SQL Server
- Entity Framework Core

### تشغيل الخدمة
```bash
cd Suppliers.Api
dotnet run
```

### الوصول للخدمة
- **Swagger UI**: https://localhost:7051/swagger
- **API Base URL**: https://localhost:7051

## 🔐 الأمان

### المصادقة
- JWT Bearer Token
- متطلبات صلاحيات Manager+ للوصول

### الصلاحيات المطلوبة
- **GET**: Manager, Admin, SuperAdmin, User
- **POST/PUT/DELETE**: Manager, Admin, SuperAdmin

## 📊 نماذج البيانات

### CreateSupplierRequest
```json
{
  "supplierName": "string",
  "phone": "string",
  "email": "string",
  "address": "string",
  "websit": "string",
  "countryId": 0,
  "supplierNote": "string",
  "attachmentId": 0
}
```

### CreateOfferRequest
```json
{
  "offerName": "string",
  "offerDescription": "string",
  "supplierID": 0,
  "offerStatusId": 0,
  "attachmentId": 0,
  "expiryDate": "2024-01-01T00:00:00Z"
}
```

## 🔍 أمثلة الاستخدام

### إنشاء مورد جديد
```http
POST https://localhost:7051/api/suppliers
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
  "supplierName": "شركة الموردين المحدودة",
  "phone": "+966501234567",
  "email": "info@supplier.com",
  "address": "الرياض، المملكة العربية السعودية",
  "websit": "https://supplier.com",
  "countryId": 1,
  "supplierNote": "مورد موثوق"
}
```

### البحث في الموردين
```http
GET https://localhost:7051/api/suppliers/search?name=شركة&email=supplier
Authorization: Bearer {your-jwt-token}
```

### عرض عروض مورد
```http
GET https://localhost:7051/api/suppliers/1/offers
Authorization: Bearer {your-jwt-token}
```

## 🐛 استكشاف الأخطاء

### مشاكل شائعة
1. **Database Connection Failed**: تحقق من connection string
2. **JWT Token Invalid**: تحقق من JWT configuration
3. **Entity Not Found**: تحقق من وجود البيانات في قاعدة البيانات

### السجلات
- Console logs
- Entity Framework logs
- JWT authentication logs

## 📝 ملاحظات مهمة

1. **Soft Delete**: الموردين يتم حذفهم بـ soft delete (IsActive = false)
2. **Relationships**: العروض مرتبطة بالموردين
3. **Validation**: جميع البيانات تمر بمرحلة التحقق
4. **Logging**: جميع العمليات مسجلة
5. **Error Handling**: معالجة شاملة للأخطاء
