using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Data;
using OrderMicroservice.Models;
using OrderMicroservice.DTOs;

namespace OrderMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(OrderDbContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Order Operations

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(
            [FromQuery] int? customerId = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] OrderType? orderType = null,
            [FromQuery] OrderStatus? orderStatus = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var query = _context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderStatusHistories)
                    .Where(o => o.IsActive);

                if (customerId.HasValue)
                    query = query.Where(o => o.CustomerId == customerId.Value);

                if (supplierId.HasValue)
                    query = query.Where(o => o.SupplierId == supplierId.Value);

                if (orderType.HasValue)
                    query = query.Where(o => o.OrderType == orderType.Value);

                if (orderStatus.HasValue)
                    query = query.Where(o => o.OrderStatus == orderStatus.Value);

                if (fromDate.HasValue)
                    query = query.Where(o => o.OrderDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(o => o.OrderDate <= toDate.Value);

                var orders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderStatusHistories)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("from-offer")]
        public async Task<ActionResult<Order>> CreateOrderFromOffer([FromBody] CreateOrderFromOfferDto dto)
        {
            try
            {
                // Generate unique order number
                var orderNumber = await GenerateOrderNumber(dto.OrderType);

                var order = new Order
                {
                    OrderNumber = orderNumber,
                    OrderType = dto.OrderType,
                    CustomerId = dto.CustomerId,
                    SupplierId = dto.SupplierId,
                    OfferId = dto.OfferId,
                    OrderDate = DateTime.UtcNow,
                    RequiredDate = dto.RequiredDate,
                    OrderStatus = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending,
                    ShippingAddress = dto.ShippingAddress,
                    BillingAddress = dto.BillingAddress,
                    PaymentMethod = dto.PaymentMethod,
                    Notes = dto.Notes,
                    CreatedBy = dto.CreatedBy,
                    CreatedByName = dto.CreatedByName,
                    IsActive = true
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add order items from offer
                if (dto.OrderItems != null && dto.OrderItems.Any())
                {
                    foreach (var itemDto in dto.OrderItems)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ItemId = itemDto.ItemId,
                            ItemName = itemDto.ItemName,
                            ItemCode = itemDto.ItemCode,
                            Description = itemDto.Description,
                            Quantity = itemDto.Quantity,
                            UnitPrice = itemDto.UnitPrice,
                            DiscountPercentage = itemDto.DiscountPercentage,
                            DiscountAmount = itemDto.DiscountAmount,
                            TaxRate = itemDto.TaxRate,
                            TaxAmount = itemDto.TaxAmount,
                            LineTotal = itemDto.LineTotal,
                            WarehouseId = itemDto.WarehouseId,
                            Location = itemDto.Location,
                            IsActive = true
                        };

                        _context.OrderItems.Add(orderItem);
                    }

                    // Calculate totals
                    await CalculateOrderTotals(order.OrderId);
                }

                // Create initial status history
                var statusHistory = new OrderStatusHistory
                {
                    OrderId = order.OrderId,
                    PreviousStatus = OrderStatus.Pending,
                    NewStatus = OrderStatus.Pending,
                    Comments = "Order created from offer",
                    ChangedBy = dto.CreatedBy,
                    ChangedByName = dto.CreatedByName,
                    ChangedDate = DateTime.UtcNow
                };

                _context.OrderStatusHistories.Add(statusHistory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Order {OrderNumber} created from offer {OfferId} for customer {CustomerId}", 
                    orderNumber, dto.OfferId, dto.CustomerId);

                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order from offer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderDto dto)
        {
            try
            {
                var orderNumber = await GenerateOrderNumber(dto.OrderType);

                var order = new Order
                {
                    OrderNumber = orderNumber,
                    OrderType = dto.OrderType,
                    CustomerId = dto.CustomerId,
                    SupplierId = dto.SupplierId,
                    OfferId = dto.OfferId,
                    OrderDate = DateTime.UtcNow,
                    RequiredDate = dto.RequiredDate,
                    OrderStatus = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending,
                    ShippingAddress = dto.ShippingAddress,
                    BillingAddress = dto.BillingAddress,
                    PaymentMethod = dto.PaymentMethod,
                    Notes = dto.Notes,
                    CreatedBy = dto.CreatedBy,
                    CreatedByName = dto.CreatedByName,
                    IsActive = true
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add order items
                if (dto.OrderItems != null && dto.OrderItems.Any())
                {
                    foreach (var itemDto in dto.OrderItems)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ItemId = itemDto.ItemId,
                            ItemName = itemDto.ItemName,
                            ItemCode = itemDto.ItemCode,
                            Description = itemDto.Description,
                            Quantity = itemDto.Quantity,
                            UnitPrice = itemDto.UnitPrice,
                            DiscountPercentage = itemDto.DiscountPercentage,
                            DiscountAmount = itemDto.DiscountAmount,
                            TaxRate = itemDto.TaxRate,
                            TaxAmount = itemDto.TaxAmount,
                            LineTotal = itemDto.LineTotal,
                            WarehouseId = itemDto.WarehouseId,
                            Location = itemDto.Location,
                            IsActive = true
                        };

                        _context.OrderItems.Add(orderItem);
                    }

                    await CalculateOrderTotals(order.OrderId);
                }

                // Create initial status history
                var statusHistory = new OrderStatusHistory
                {
                    OrderId = order.OrderId,
                    PreviousStatus = OrderStatus.Pending,
                    NewStatus = OrderStatus.Pending,
                    Comments = "Order created",
                    ChangedBy = dto.CreatedBy,
                    ChangedByName = dto.CreatedByName,
                    ChangedDate = DateTime.UtcNow
                };

                _context.OrderStatusHistories.Add(statusHistory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Order {OrderNumber} created for customer {CustomerId}", 
                    orderNumber, dto.CustomerId);

                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                var previousStatus = order.OrderStatus;
                order.OrderStatus = dto.NewStatus;
                order.UpdatedDate = DateTime.UtcNow;

                // Update specific dates based on status
                switch (dto.NewStatus)
                {
                    case OrderStatus.Confirmed:
                        // No specific date update
                        break;
                    case OrderStatus.Processing:
                        // No specific date update
                        break;
                    case OrderStatus.Shipped:
                        order.ShippedDate = DateTime.UtcNow;
                        break;
                    case OrderStatus.Delivered:
                        order.DeliveredDate = DateTime.UtcNow;
                        break;
                    case OrderStatus.Completed:
                        order.DeliveredDate = DateTime.UtcNow;
                        break;
                }

                // Create status history entry
                var statusHistory = new OrderStatusHistory
                {
                    OrderId = order.OrderId,
                    PreviousStatus = previousStatus,
                    NewStatus = dto.NewStatus,
                    Comments = dto.Comments,
                    ChangedBy = dto.ChangedBy,
                    ChangedByName = dto.ChangedByName,
                    ChangedDate = DateTime.UtcNow
                };

                _context.OrderStatusHistories.Add(statusHistory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Order {OrderId} status updated from {PreviousStatus} to {NewStatus}", 
                    id, previousStatus, dto.NewStatus);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                // Only allow updates if order is in pending or confirmed status
                if (order.OrderStatus != OrderStatus.Pending && order.OrderStatus != OrderStatus.Confirmed)
                {
                    return BadRequest("Cannot update order in current status");
                }

                order.RequiredDate = dto.RequiredDate;
                order.ShippingAddress = dto.ShippingAddress;
                order.BillingAddress = dto.BillingAddress;
                order.PaymentMethod = dto.PaymentMethod;
                order.Notes = dto.Notes;
                order.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Order {OrderId} updated", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                // Only allow cancellation if order is pending or confirmed
                if (order.OrderStatus != OrderStatus.Pending && order.OrderStatus != OrderStatus.Confirmed)
                {
                    return BadRequest("Cannot cancel order in current status");
                }

                var previousStatus = order.OrderStatus;
                order.OrderStatus = OrderStatus.Cancelled;
                order.UpdatedDate = DateTime.UtcNow;

                // Create status history entry
                var statusHistory = new OrderStatusHistory
                {
                    OrderId = order.OrderId,
                    PreviousStatus = previousStatus,
                    NewStatus = OrderStatus.Cancelled,
                    Comments = "Order cancelled",
                    ChangedDate = DateTime.UtcNow
                };

                _context.OrderStatusHistories.Add(statusHistory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Order {OrderId} cancelled", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Order Item Operations

        [HttpGet("{orderId}/items")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int orderId)
        {
            try
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == orderId && oi.IsActive)
                    .ToListAsync();

                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order items for order {OrderId}", orderId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<OrderItem>> AddOrderItem(int orderId, [FromBody] CreateOrderItemDto dto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                // Only allow adding items if order is pending or confirmed
                if (order.OrderStatus != OrderStatus.Pending && order.OrderStatus != OrderStatus.Confirmed)
                {
                    return BadRequest("Cannot add items to order in current status");
                }

                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ItemId = dto.ItemId,
                    ItemName = dto.ItemName,
                    ItemCode = dto.ItemCode,
                    Description = dto.Description,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    DiscountPercentage = dto.DiscountPercentage,
                    DiscountAmount = dto.DiscountAmount,
                    TaxRate = dto.TaxRate,
                    TaxAmount = dto.TaxAmount,
                    LineTotal = dto.LineTotal,
                    WarehouseId = dto.WarehouseId,
                    Location = dto.Location,
                    IsActive = true
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                // Recalculate order totals
                await CalculateOrderTotals(orderId);

                _logger.LogInformation("Order item added to order {OrderId}", orderId);

                return CreatedAtAction(nameof(GetOrderItems), new { orderId }, orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order item to order {OrderId}", orderId);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Helper Methods

        private async Task<string> GenerateOrderNumber(OrderType orderType)
        {
            var prefix = orderType switch
            {
                OrderType.Purchase => "PO",
                OrderType.Sales => "SO",
                OrderType.Transfer => "TO",
                OrderType.Return => "RO",
                _ => "OR"
            };

            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var count = await _context.Orders
                .Where(o => o.OrderNumber.StartsWith($"{prefix}{today}"))
                .CountAsync();

            return $"{prefix}{today}{(count + 1):D4}";
        }

        private async Task CalculateOrderTotals(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return;

            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId && oi.IsActive)
                .ToListAsync();

            order.SubTotal = orderItems.Sum(oi => oi.LineTotal);
            order.TaxAmount = orderItems.Sum(oi => oi.TaxAmount);
            order.DiscountAmount = orderItems.Sum(oi => oi.DiscountAmount);
            order.TotalAmount = order.SubTotal + order.TaxAmount + order.ShippingCost - order.DiscountAmount;

            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
