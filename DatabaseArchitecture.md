# SCM Microservices Database Architecture

## 🏗️ **Database per Service Pattern**

### **Current Architecture:**
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Suppliers.Api  │    │ InventoryMicro  │    │ OrderMicro      │
│                 │    │                 │    │                 │
└─────────┬───────┘    └─────────┬───────┘    └─────────┬───────┘
          │                      │                      │
          └──────────────────────┼──────────────────────┘
                                 │
                    ┌─────────────▼─────────────┐
                    │        SCMDB              │
                    │    (Shared Database)      │
                    └───────────────────────────┘
```

### **Recommended Architecture:**
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Suppliers.Api  │    │ InventoryMicro  │    │ OrderMicro      │
│                 │    │                 │    │                 │
└─────────┬───────┘    └─────────┬───────┘    └─────────┬───────┘
          │                      │                      │
    ┌─────▼─────┐          ┌─────▼─────┐          ┌─────▼─────┐
    │SuppliersDB│          │InventoryDB│          │  OrderDB  │
    └───────────┘          └───────────┘          └───────────┘
```

## 📊 **Database Mapping**

| Microservice | Database | Purpose |
|--------------|----------|---------|
| **Suppliers.Api** | `SuppliersDB` | Suppliers, Offers, Items, Contracts |
| **InventoryMicroservice** | `InventoryDB` | Warehouses, Stock, Movements, Alerts |
| **OrderMicroservice** | `OrderDB` | Orders, OrderItems, Status History |
| **IdentityMicroservice** | `IdentityDB` | Users, Roles, Authentication |
| **CustomerMicroservice** | `CustomerDB` | Customer profiles, preferences |
| **AdminMicroservice** | `AdminDB` | System configuration, admin data |

## 🔄 **Data Synchronization Strategies**

### **1. Event-Driven Communication**
```csharp
// When order is created, publish event
public async Task CreateOrder(Order order)
{
    await _context.Orders.AddAsync(order);
    await _context.SaveChangesAsync();
    
    // Publish event for inventory service
    await _eventBus.PublishAsync(new OrderCreatedEvent
    {
        OrderId = order.OrderId,
        Items = order.OrderItems.Select(oi => new OrderItemEvent
        {
            ItemId = oi.ItemId,
            Quantity = oi.Quantity,
            WarehouseId = oi.WarehouseId
        }).ToList()
    });
}
```

### **2. API Communication**
```csharp
// Order service calls Inventory service
public async Task<bool> CheckStockAvailability(int itemId, int warehouseId, decimal quantity)
{
    var response = await _httpClient.GetAsync(
        $"/api/inventory/items/availability?itemId={itemId}&warehouseId={warehouseId}&quantity={quantity}");
    
    return response.IsSuccessStatusCode;
}
```

### **3. Shared Reference Data**
```csharp
// Shared reference data (Items, Currencies, Countries)
public class SharedReferenceData
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemCode { get; set; }
    public DateTime LastUpdated { get; set; }
}
```

## 🛠️ **Implementation Steps**

### **Step 1: Update Connection Strings**
```json
{
  "ConnectionStrings": {
    "ConUser": "Data Source=.;Initial Catalog=SuppliersDB;Persist Security Info=True;User ID=test;Password=123;Encrypt=True;Trust Server Certificate=True"
  }
}
```

### **Step 2: Run Database Setup**
```sql
-- Execute DatabaseSetup.sql to create all databases
sqlcmd -S . -i DatabaseSetup.sql
```

### **Step 3: Update Entity Framework Migrations**
```bash
# For each microservice
dotnet ef migrations add InitialCreate --context SuppliersDbContext
dotnet ef database update --context SuppliersDbContext
```

### **Step 4: Implement Event Bus (Optional)**
```csharp
// Add to Program.cs
builder.Services.AddScoped<IEventBus, RabbitMQEventBus>();
```

## 🔒 **Security Considerations**

### **Database Access Control**
```sql
-- Create service-specific users
CREATE LOGIN [SuppliersService] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [SuppliersService] FOR LOGIN [SuppliersService];

-- Grant permissions only to specific database
USE SuppliersDB;
ALTER ROLE db_datareader ADD MEMBER [SuppliersService];
ALTER ROLE db_datawriter ADD MEMBER [SuppliersService];
```

### **Connection String Security**
```json
{
  "ConnectionStrings": {
    "ConUser": "Data Source=.;Initial Catalog=SuppliersDB;User ID=SuppliersService;Password=SecurePassword123!;Encrypt=True;Trust Server Certificate=True"
  }
}
```

## 📈 **Benefits of Database Separation**

### **1. Scalability**
- Scale each database independently
- Use different hardware for different workloads
- Optimize database settings per service

### **2. Technology Flexibility**
```csharp
// Suppliers service can use SQL Server
builder.Services.AddDbContext<SuppliersDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inventory service can use PostgreSQL
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(connectionString));

// Analytics service can use MongoDB
builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseMongoDB(connectionString));
```

### **3. Fault Isolation**
- Database failure in one service doesn't affect others
- Independent backup and recovery strategies
- Better monitoring and alerting

### **4. Team Autonomy**
- Teams can work independently
- Different release cycles
- Independent schema evolution

## ⚠️ **Challenges and Solutions**

### **Challenge 1: Data Consistency**
**Solution:** Eventual consistency with event sourcing
```csharp
public class OrderCreatedEvent : IEvent
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemEvent> Items { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### **Challenge 2: Cross-Service Queries**
**Solution:** API composition or CQRS
```csharp
public class OrderWithInventoryService
{
    public async Task<OrderWithStockInfo> GetOrderWithStockInfo(int orderId)
    {
        var order = await _orderService.GetOrderAsync(orderId);
        var stockInfo = await _inventoryService.GetStockInfoAsync(order.Items);
        
        return new OrderWithStockInfo { Order = order, StockInfo = stockInfo };
    }
}
```

### **Challenge 3: Transaction Management**
**Solution:** Saga pattern
```csharp
public class OrderProcessingSaga
{
    public async Task ProcessOrder(Order order)
    {
        try
        {
            // Step 1: Reserve inventory
            await _inventoryService.ReserveStockAsync(order.Items);
            
            // Step 2: Create order
            await _orderService.CreateOrderAsync(order);
            
            // Step 3: Process payment
            await _paymentService.ProcessPaymentAsync(order.PaymentInfo);
        }
        catch (Exception ex)
        {
            // Compensate transactions
            await CompensateOrderProcessing(order, ex);
        }
    }
}
```

## 🚀 **Migration Strategy**

### **Phase 1: Setup Separate Databases**
1. Create new databases
2. Update connection strings
3. Run migrations

### **Phase 2: Data Migration**
1. Export data from SCMDB
2. Import to respective databases
3. Verify data integrity

### **Phase 3: Update Services**
1. Deploy updated microservices
2. Test cross-service communication
3. Monitor performance

### **Phase 4: Cleanup**
1. Remove shared database dependencies
2. Archive old SCMDB
3. Update documentation

## 📋 **Next Steps**

1. **Execute DatabaseSetup.sql** to create all databases
2. **Update connection strings** in all microservices
3. **Run EF migrations** for each service
4. **Test database connectivity** for each service
5. **Implement event-driven communication** (optional)
6. **Set up monitoring** for each database
7. **Create backup strategies** for each database

This architecture provides better scalability, maintainability, and fault tolerance for your SCM system!
