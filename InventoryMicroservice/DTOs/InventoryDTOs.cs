using System.ComponentModel.DataAnnotations;
using InventoryMicroservice.Models;

namespace InventoryMicroservice.DTOs
{
    // Warehouse DTOs
    public class CreateWarehouseDto
    {
        [Required]
        [MaxLength(100)]
        public string WarehouseName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? ManagerName { get; set; }

        [MaxLength(50)]
        public string? ManagerPhone { get; set; }

        [MaxLength(50)]
        public string? ManagerEmail { get; set; }

        public decimal? Capacity { get; set; }
    }

    public class UpdateWarehouseDto
    {
        [Required]
        [MaxLength(100)]
        public string WarehouseName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? ManagerName { get; set; }

        [MaxLength(50)]
        public string? ManagerPhone { get; set; }

        [MaxLength(50)]
        public string? ManagerEmail { get; set; }

        public decimal? Capacity { get; set; }
    }

    // Inventory Item DTOs
    public class CreateInventoryItemDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Initial stock must be non-negative")]
        public decimal InitialStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Reorder point must be non-negative")]
        public decimal? ReorderPoint { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max stock must be non-negative")]
        public decimal? MaxStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be non-negative")]
        public decimal? UnitCost { get; set; }

        [MaxLength(50)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? BinNumber { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }
    }

    public class UpdateInventoryItemDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "Reorder point must be non-negative")]
        public decimal? ReorderPoint { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max stock must be non-negative")]
        public decimal? MaxStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be non-negative")]
        public decimal? UnitCost { get; set; }

        [MaxLength(50)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? BinNumber { get; set; }
    }

    // Stock Movement DTOs
    public class CreateStockMovementDto
    {
        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        public MovementType MovementType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be non-negative")]
        public decimal? UnitCost { get; set; }

        [MaxLength(200)]
        public string? Reference { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int? CreatedBy { get; set; }

        [MaxLength(50)]
        public string? CreatedByName { get; set; }

        [MaxLength(50)]
        public string? BatchNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? SupplierId { get; set; }

        public int? CustomerId { get; set; }
    }

    // Stock Alert DTOs
    public class CreateStockAlertDto
    {
        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        public AlertType AlertType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Current stock must be non-negative")]
        public decimal CurrentStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Threshold value must be non-negative")]
        public decimal? ThresholdValue { get; set; }
    }

    public class ResolveStockAlertDto
    {
        [MaxLength(200)]
        public string? ResolutionNotes { get; set; }

        public int? ResolvedBy { get; set; }
    }

    // Response DTOs
    public class InventorySummaryDto
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int OverstockItems { get; set; }
        public decimal TotalValue { get; set; }
        public int UnreadAlerts { get; set; }
    }

    public class StockMovementSummaryDto
    {
        public int TotalMovements { get; set; }
        public decimal TotalInbound { get; set; }
        public decimal TotalOutbound { get; set; }
        public decimal NetMovement { get; set; }
        public DateTime LastMovementDate { get; set; }
    }
}
