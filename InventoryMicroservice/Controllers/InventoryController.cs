using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryMicroservice.Data;
using InventoryMicroservice.Models;
using InventoryMicroservice.DTOs;

namespace InventoryMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(InventoryDbContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Warehouse Operations

        [HttpGet("warehouses")]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            try
            {
                var warehouses = await _context.Warehouses
                    .Where(w => w.IsActive)
                    .OrderBy(w => w.WarehouseName)
                    .ToListAsync();

                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving warehouses");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("warehouses/{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving warehouse {WarehouseId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("warehouses")]
        public async Task<ActionResult<Warehouse>> CreateWarehouse([FromBody] CreateWarehouseDto dto)
        {
            try
            {
                var warehouse = new Warehouse
                {
                    WarehouseName = dto.WarehouseName,
                    Address = dto.Address,
                    City = dto.City,
                    Country = dto.Country,
                    PostalCode = dto.PostalCode,
                    ManagerName = dto.ManagerName,
                    ManagerPhone = dto.ManagerPhone,
                    ManagerEmail = dto.ManagerEmail,
                    Capacity = dto.Capacity,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Warehouses.Add(warehouse);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Warehouse {WarehouseName} created with ID {WarehouseId}", 
                    warehouse.WarehouseName, warehouse.WarehouseId);

                return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.WarehouseId }, warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating warehouse");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("warehouses/{id}")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] UpdateWarehouseDto dto)
        {
            try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                warehouse.WarehouseName = dto.WarehouseName;
                warehouse.Address = dto.Address;
                warehouse.City = dto.City;
                warehouse.Country = dto.Country;
                warehouse.PostalCode = dto.PostalCode;
                warehouse.ManagerName = dto.ManagerName;
                warehouse.ManagerPhone = dto.ManagerPhone;
                warehouse.ManagerEmail = dto.ManagerEmail;
                warehouse.Capacity = dto.Capacity;
                warehouse.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Warehouse {WarehouseId} updated", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating warehouse {WarehouseId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("warehouses/{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                warehouse.IsActive = false;
                warehouse.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Warehouse {WarehouseId} deactivated", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting warehouse {WarehouseId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Inventory Item Operations

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems(
            [FromQuery] int? warehouseId = null,
            [FromQuery] int? itemId = null)
        {
            try
            {
                var query = _context.InventoryItems
                    .Include(ii => ii.Warehouse)
                    .Where(ii => ii.IsActive);

                if (warehouseId.HasValue)
                    query = query.Where(ii => ii.WarehouseId == warehouseId.Value);

                if (itemId.HasValue)
                    query = query.Where(ii => ii.ItemId == itemId.Value);

                var items = await query.ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventory items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            try
            {
                var item = await _context.InventoryItems
                    .Include(ii => ii.Warehouse)
                    .FirstOrDefaultAsync(ii => ii.InventoryItemId == id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventory item {ItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("items")]
        public async Task<ActionResult<InventoryItem>> CreateInventoryItem([FromBody] CreateInventoryItemDto dto)
        {
            try
            {
                // Check if item already exists in warehouse
                var existingItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(ii => ii.ItemId == dto.ItemId && ii.WarehouseId == dto.WarehouseId);

                if (existingItem != null)
                {
                    return BadRequest("Item already exists in this warehouse");
                }

                var inventoryItem = new InventoryItem
                {
                    ItemId = dto.ItemId,
                    WarehouseId = dto.WarehouseId,
                    CurrentStock = dto.InitialStock,
                    AvailableStock = dto.InitialStock,
                    ReorderPoint = dto.ReorderPoint,
                    MaxStock = dto.MaxStock,
                    UnitCost = dto.UnitCost,
                    Location = dto.Location,
                    BinNumber = dto.BinNumber,
                    IsActive = true,
                    LastUpdated = DateTime.UtcNow
                };

                _context.InventoryItems.Add(inventoryItem);
                await _context.SaveChangesAsync();

                // Create initial stock movement
                var movement = new InventoryMovement
                {
                    InventoryItemId = inventoryItem.InventoryItemId,
                    MovementType = MovementType.In,
                    Quantity = dto.InitialStock,
                    UnitCost = dto.UnitCost,
                    TotalCost = dto.UnitCost * dto.InitialStock,
                    Reference = "Initial Stock",
                    Description = "Initial stock entry",
                    MovementDate = DateTime.UtcNow,
                    CreatedByName = dto.CreatedBy
                };

                _context.InventoryMovements.Add(movement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inventory item created for ItemId {ItemId} in Warehouse {WarehouseId}", 
                    dto.ItemId, dto.WarehouseId);

                return CreatedAtAction(nameof(GetInventoryItem), new { id = inventoryItem.InventoryItemId }, inventoryItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inventory item");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Stock Movement Operations

        [HttpPost("movements")]
        public async Task<ActionResult<InventoryMovement>> CreateStockMovement([FromBody] CreateStockMovementDto dto)
        {
            try
            {
                var inventoryItem = await _context.InventoryItems.FindAsync(dto.InventoryItemId);

                if (inventoryItem == null)
                {
                    return NotFound("Inventory item not found");
                }

                // Validate stock availability for outbound movements
                if (dto.MovementType == MovementType.Out || dto.MovementType == MovementType.Transfer)
                {
                    if (inventoryItem.AvailableStock < dto.Quantity)
                    {
                        return BadRequest("Insufficient stock available");
                    }
                }

                var movement = new InventoryMovement
                {
                    InventoryItemId = dto.InventoryItemId,
                    MovementType = dto.MovementType,
                    Quantity = dto.Quantity,
                    UnitCost = dto.UnitCost,
                    TotalCost = dto.UnitCost * dto.Quantity,
                    Reference = dto.Reference,
                    Description = dto.Description,
                    MovementDate = DateTime.UtcNow,
                    CreatedBy = dto.CreatedBy,
                    CreatedByName = dto.CreatedByName,
                    BatchNumber = dto.BatchNumber,
                    ExpiryDate = dto.ExpiryDate,
                    SupplierId = dto.SupplierId,
                    CustomerId = dto.CustomerId
                };

                _context.InventoryMovements.Add(movement);

                // Update inventory levels
                switch (dto.MovementType)
                {
                    case MovementType.In:
                    case MovementType.Return:
                        inventoryItem.CurrentStock += dto.Quantity;
                        inventoryItem.AvailableStock += dto.Quantity;
                        break;
                    case MovementType.Out:
                    case MovementType.Damage:
                    case MovementType.Expired:
                        inventoryItem.CurrentStock -= dto.Quantity;
                        inventoryItem.AvailableStock -= dto.Quantity;
                        break;
                    case MovementType.Reserved:
                        inventoryItem.ReservedStock += dto.Quantity;
                        inventoryItem.AvailableStock -= dto.Quantity;
                        break;
                    case MovementType.Unreserved:
                        inventoryItem.ReservedStock -= dto.Quantity;
                        inventoryItem.AvailableStock += dto.Quantity;
                        break;
                }

                inventoryItem.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Check for stock alerts
                await CheckStockAlerts(inventoryItem);

                _logger.LogInformation("Stock movement created: {MovementType} {Quantity} for Item {ItemId}", 
                    dto.MovementType, dto.Quantity, dto.InventoryItemId);

                return CreatedAtAction(nameof(GetStockMovement), new { id = movement.MovementId }, movement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock movement");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("movements/{id}")]
        public async Task<ActionResult<InventoryMovement>> GetStockMovement(int id)
        {
            try
            {
                var movement = await _context.InventoryMovements
                    .Include(im => im.InventoryItem)
                    .ThenInclude(ii => ii.Warehouse)
                    .FirstOrDefaultAsync(im => im.MovementId == id);

                if (movement == null)
                {
                    return NotFound();
                }

                return Ok(movement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock movement {MovementId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("movements")]
        public async Task<ActionResult<IEnumerable<InventoryMovement>>> GetStockMovements(
            [FromQuery] int? inventoryItemId = null,
            [FromQuery] MovementType? movementType = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var query = _context.InventoryMovements
                    .Include(im => im.InventoryItem)
                    .ThenInclude(ii => ii.Warehouse)
                    .AsQueryable();

                if (inventoryItemId.HasValue)
                    query = query.Where(im => im.InventoryItemId == inventoryItemId.Value);

                if (movementType.HasValue)
                    query = query.Where(im => im.MovementType == movementType.Value);

                if (fromDate.HasValue)
                    query = query.Where(im => im.MovementDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(im => im.MovementDate <= toDate.Value);

                var movements = await query
                    .OrderByDescending(im => im.MovementDate)
                    .ToListAsync();

                return Ok(movements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock movements");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Stock Alerts

        [HttpGet("alerts")]
        public async Task<ActionResult<IEnumerable<StockAlert>>> GetStockAlerts(
            [FromQuery] bool? isRead = null,
            [FromQuery] AlertType? alertType = null)
        {
            try
            {
                var query = _context.StockAlerts
                    .Include(sa => sa.InventoryItem)
                    .ThenInclude(ii => ii.Warehouse)
                    .AsQueryable();

                if (isRead.HasValue)
                    query = query.Where(sa => sa.IsRead == isRead.Value);

                if (alertType.HasValue)
                    query = query.Where(sa => sa.AlertType == alertType.Value);

                var alerts = await query
                    .OrderByDescending(sa => sa.AlertDate)
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock alerts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("alerts/{id}/read")]
        public async Task<IActionResult> MarkAlertAsRead(int id)
        {
            try
            {
                var alert = await _context.StockAlerts.FindAsync(id);

                if (alert == null)
                {
                    return NotFound();
                }

                alert.IsRead = true;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking alert as read {AlertId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Helper Methods

        private async Task CheckStockAlerts(InventoryItem inventoryItem)
        {
            try
            {
                // Check for low stock
                if (inventoryItem.ReorderPoint.HasValue && 
                    inventoryItem.AvailableStock <= inventoryItem.ReorderPoint.Value)
                {
                    var existingAlert = await _context.StockAlerts
                        .FirstOrDefaultAsync(sa => sa.InventoryItemId == inventoryItem.InventoryItemId &&
                                                  sa.AlertType == AlertType.LowStock &&
                                                  !sa.IsResolved);

                    if (existingAlert == null)
                    {
                        var alert = new StockAlert
                        {
                            InventoryItemId = inventoryItem.InventoryItemId,
                            AlertType = AlertType.LowStock,
                            Message = $"Low stock alert: {inventoryItem.AvailableStock} units remaining (Reorder point: {inventoryItem.ReorderPoint})",
                            CurrentStock = inventoryItem.AvailableStock,
                            ThresholdValue = inventoryItem.ReorderPoint,
                            AlertDate = DateTime.UtcNow
                        };

                        _context.StockAlerts.Add(alert);
                    }
                }

                // Check for out of stock
                if (inventoryItem.AvailableStock <= 0)
                {
                    var existingAlert = await _context.StockAlerts
                        .FirstOrDefaultAsync(sa => sa.InventoryItemId == inventoryItem.InventoryItemId &&
                                                  sa.AlertType == AlertType.OutOfStock &&
                                                  !sa.IsResolved);

                    if (existingAlert == null)
                    {
                        var alert = new StockAlert
                        {
                            InventoryItemId = inventoryItem.InventoryItemId,
                            AlertType = AlertType.OutOfStock,
                            Message = "Out of stock alert: No units available",
                            CurrentStock = inventoryItem.AvailableStock,
                            AlertDate = DateTime.UtcNow
                        };

                        _context.StockAlerts.Add(alert);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking stock alerts for item {ItemId}", inventoryItem.InventoryItemId);
            }
        }

        #endregion
    }
}
