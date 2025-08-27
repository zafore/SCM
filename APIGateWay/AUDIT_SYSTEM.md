# 📊 نظام تسجيل حركة المستخدمين - Audit System

## 📋 نظرة عامة

نظام Audit System متقدم لتسجيل جميع حركات المستخدمين في النظام، بما في ذلك:
- **الدخول والخروج** (Login/Logout)
- **إنشاء البيانات** (Create)
- **تعديل البيانات** (Update)
- **حذف البيانات** (Delete)
- **عرض البيانات** (View)

## 🎯 الميزات

### ✅ **تسجيل شامل**
- جميع العمليات مسجلة تلقائياً
- تفاصيل المستخدم والجلسة
- معلومات الطلب والاستجابة
- أوقات التنفيذ والأخطاء

### ✅ **تصنيف العمليات**
- **LOGIN/LOGOUT**: تسجيل الدخول والخروج
- **CREATE**: إنشاء بيانات جديدة
- **UPDATE**: تعديل البيانات الموجودة
- **DELETE**: حذف البيانات
- **VIEW**: عرض البيانات

### ✅ **تصنيف الكيانات**
- **User**: المستخدمين
- **Supplier**: الموردين
- **Offer**: العروض
- **Inventory**: المخزون
- **Order**: الطلبات
- **Payment**: المدفوعات
- **Accounting**: المحاسبة

### ✅ **أمان متقدم**
- صلاحيات مختلفة حسب الدور
- حماية البيانات الحساسة
- تسجيل محاولات الوصول غير المصرح بها

## 🔧 التكوين

### **Middleware Configuration**
```csharp
// في Program.cs
app.UseMiddleware<APIGateWay.Middleware.AuditMiddleware>();
```

### **Service Registration**
```csharp
builder.Services.AddScoped<IAuditService, AuditService>();
```

### **Session Support**
```csharp
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
```

## 📊 البيانات المسجلة

### **معلومات المستخدم**
- User ID
- User Name
- User Email
- Role
- Session ID

### **معلومات العملية**
- Action (LOGIN, CREATE, UPDATE, DELETE, VIEW)
- Entity Type (User, Supplier, Offer, etc.)
- Entity ID
- Entity Name
- HTTP Method
- Endpoint

### **معلومات الشبكة**
- IP Address
- User Agent
- Request Data
- Response Data
- Status Code

### **معلومات التوقيت**
- Timestamp
- Duration
- Error Messages

## 🔗 API Endpoints

### **إحصائيات النظام**
```http
GET /api/audit/statistics
Authorization: Bearer {token}
```

### **السجلات الحديثة**
```http
GET /api/audit/recent?page=1&pageSize=20
Authorization: Bearer {token}
```

### **سجلات المستخدم**
```http
GET /api/audit/user/{userId}?page=1&pageSize=50
Authorization: Bearer {token}
```

### **سجلات الكيان**
```http
GET /api/audit/entity/{entityType}?entityId={id}&page=1&pageSize=50
Authorization: Bearer {token}
```

### **سجلات حسب التاريخ**
```http
GET /api/audit/date-range?startDate=2024-01-01&endDate=2024-12-31&page=1&pageSize=50
Authorization: Bearer {token}
```

### **سجلات حسب العملية**
```http
GET /api/audit/action/{action}?page=1&pageSize=50
Authorization: Bearer {token}
```

### **سجل محدد**
```http
GET /api/audit/{id}
Authorization: Bearer {token}
```

### **سجلاتي الشخصية**
```http
GET /api/audit/my-logs?page=1&pageSize=50
Authorization: Bearer {token}
```

## 🔐 الصلاحيات

### **SuperAdmin**
- ✅ **عرض جميع السجلات**
- ✅ **حذف السجلات**
- ✅ **إحصائيات النظام**
- ✅ **سجلات جميع المستخدمين**

### **Admin**
- ✅ **عرض السجلات العامة**
- ✅ **سجلات المستخدمين**
- ✅ **سجلات الكيانات**
- ❌ **لا يمكنه حذف السجلات**

### **Manager**
- ✅ **عرض سجلاته الشخصية**
- ✅ **سجلات الكيانات المتاحة**
- ❌ **لا يمكنه عرض سجلات المستخدمين الآخرين**

### **User**
- ✅ **عرض سجلاته الشخصية فقط**
- ❌ **لا يمكنه الوصول للسجلات الأخرى**

## 📝 أمثلة الاستخدام

### **1. عرض إحصائيات النظام**
```http
GET https://localhost:7034/api/audit/statistics
Authorization: Bearer {admin-token}

Response:
{
  "totalLogs": 1250,
  "loginCount": 45,
  "createCount": 120,
  "updateCount": 85,
  "deleteCount": 15,
  "generatedAt": "2024-01-15T10:30:00Z"
}
```

### **2. عرض السجلات الحديثة**
```http
GET https://localhost:7034/api/audit/recent
Authorization: Bearer {manager-token}

Response:
{
  "data": [
    {
      "id": 1,
      "userId": "user123",
      "userName": "Ahmed Ali",
      "action": "CREATE",
      "entityType": "Supplier",
      "entityName": "New Supplier",
      "timestamp": "2024-01-15T10:25:00Z",
      "ipAddress": "192.168.1.100"
    }
  ],
  "page": 1,
  "pageSize": 20,
  "timeRange": "Last 24 hours"
}
```

### **3. عرض سجلات مورد محدد**
```http
GET https://localhost:7034/api/audit/entity/Supplier?entityId=1
Authorization: Bearer {admin-token}

Response:
{
  "data": [
    {
      "id": 5,
      "userId": "user123",
      "userName": "Ahmed Ali",
      "action": "CREATE",
      "entityType": "Supplier",
      "entityId": "1",
      "entityName": "Supplier ABC",
      "timestamp": "2024-01-15T09:00:00Z"
    },
    {
      "id": 8,
      "userId": "user456",
      "userName": "Sara Mohamed",
      "action": "UPDATE",
      "entityType": "Supplier",
      "entityId": "1",
      "entityName": "Supplier ABC",
      "changes": "{\"phone\": \"+966501234567\"}",
      "timestamp": "2024-01-15T10:00:00Z"
    }
  ]
}
```

### **4. عرض سجلات الدخول**
```http
GET https://localhost:7034/api/audit/action/LOGIN
Authorization: Bearer {admin-token}

Response:
{
  "data": [
    {
      "id": 10,
      "userId": "user123",
      "userName": "Ahmed Ali",
      "action": "LOGIN",
      "entityType": "User",
      "ipAddress": "192.168.1.100",
      "userAgent": "Mozilla/5.0...",
      "timestamp": "2024-01-15T08:30:00Z"
    }
  ]
}
```

## 🔍 البحث والفلترة

### **البحث حسب المستخدم**
```http
GET /api/audit/user/{userId}?page=1&pageSize=50
```

### **البحث حسب الكيان**
```http
GET /api/audit/entity/{entityType}?entityId={id}
```

### **البحث حسب التاريخ**
```http
GET /api/audit/date-range?startDate=2024-01-01&endDate=2024-01-31
```

### **البحث حسب العملية**
```http
GET /api/audit/action/{action}
```

## 📊 التقارير

### **تقرير نشاط المستخدم**
```http
GET /api/audit/user/{userId}
```

### **تقرير نشاط الكيان**
```http
GET /api/audit/entity/{entityType}
```

### **تقرير الأمان**
```http
GET /api/audit/action/LOGIN
GET /api/audit/action/DELETE
```

### **تقرير الأداء**
```http
GET /api/audit/statistics
```

## 🚨 مراقبة الأمان

### **محاولات الدخول المشبوهة**
- تسجيل IP addresses
- تتبع User Agents
- مراقبة أوقات الدخول

### **العمليات الحساسة**
- حذف البيانات
- تعديل الصلاحيات
- الوصول للبيانات الحساسة

### **الأخطاء الأمنية**
- محاولات الوصول غير المصرح بها
- أخطاء المصادقة
- انتهاكات الصلاحيات

## 🔧 التخصيص

### **إضافة حقول جديدة**
```csharp
public class AuditLog
{
    // إضافة حقول جديدة هنا
    public string? CustomField { get; set; }
}
```

### **تخصيص العمليات**
```csharp
private string GetActionFromHttpMethod(string method)
{
    return method.ToUpper() switch
    {
        "GET" => "VIEW",
        "POST" => "CREATE",
        "PUT" => "UPDATE",
        "DELETE" => "DELETE",
        "CUSTOM" => "CUSTOM_ACTION", // إضافة عمليات مخصصة
        _ => "UNKNOWN"
    };
}
```

### **تخصيص الكيانات**
```csharp
private string GetEntityTypeFromPath(PathString path)
{
    // إضافة كيانات جديدة
    return entityType switch
    {
        "custom" => "CustomEntity",
        _ => "Unknown"
    };
}
```

## 📈 الأداء

### **التخزين**
- تخزين في الذاكرة للعرض التوضيحي
- يمكن ربطه بقاعدة بيانات للإنتاج
- ضغط البيانات القديمة

### **الاستعلام**
- فهرسة حسب التاريخ
- فهرسة حسب المستخدم
- فهرسة حسب الكيان

### **الذاكرة**
- تنظيف السجلات القديمة
- ضغط البيانات
- تخزين مؤقت للاستعلامات المتكررة

## 🛠️ الصيانة

### **تنظيف السجلات**
```csharp
// حذف السجلات الأقدم من 90 يوم
var oldLogs = await _auditService.GetAuditLogsByDateRangeAsync(
    DateTime.UtcNow.AddDays(-90), 
    DateTime.UtcNow.AddDays(-1)
);
```

### **نسخ احتياطي**
- تصدير السجلات المهمة
- أرشفة البيانات القديمة
- استعادة البيانات عند الحاجة

### **مراقبة الأداء**
- تتبع حجم السجلات
- مراقبة أوقات الاستجابة
- تحسين الاستعلامات

## ⚠️ ملاحظات مهمة

1. **الخصوصية**: حماية البيانات الشخصية
2. **الأداء**: تحسين الاستعلامات الكبيرة
3. **التخزين**: إدارة مساحة التخزين
4. **الأمان**: حماية السجلات من التلاعب
5. **الامتثال**: الالتزام بالقوانين المحلية

## 🚀 التطوير المستقبلي

### **الميزات المخططة**
- [ ] واجهة مستخدم للتقارير
- [ ] تنبيهات الأمان
- [ ] تحليلات متقدمة
- [ ] تصدير التقارير
- [ ] تكامل مع أنظمة المراقبة

### **التحسينات**
- [ ] قاعدة بيانات مخصصة
- [ ] فهرسة متقدمة
- [ ] ضغط البيانات
- [ ] توزيع الأحمال
- [ ] نسخ احتياطي تلقائي
