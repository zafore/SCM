using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMicroservice.Models
{
    [Table("InventoryMovements")]
    public class InventoryMovement
    {
        [Key]
        [Column("MovementId")]
        public int MovementId { get; set; }

        [Required]
        [Column("InventoryItemId")]
        public int InventoryItemId { get; set; }

        [Required]
        [Column("MovementType")]
        public MovementType MovementType { get; set; }

        [Required]
        [Column("Quantity", TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column("UnitCost", TypeName = "decimal(18,2)")]
        public decimal? UnitCost { get; set; }

        [Column("TotalCost", TypeName = "decimal(18,2)")]
        public decimal? TotalCost { get; set; }

        [MaxLength(200)]
        [Column("Reference")]
        public string? Reference { get; set; }

        [MaxLength(200)]
        [Column("Description")]
        public string? Description { get; set; }

        [Column("MovementDate")]
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }

        [MaxLength(50)]
        [Column("CreatedByName")]
        public string? CreatedByName { get; set; }

        [Column("BatchNumber")]
        public string? BatchNumber { get; set; }

        [Column("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Column("SupplierId")]
        public int? SupplierId { get; set; }

        [Column("CustomerId")]
        public int? CustomerId { get; set; }

        // Navigation properties
        [ForeignKey("InventoryItemId")]
        public virtual InventoryItem InventoryItem { get; set; } = null!;
    }

    public enum MovementType
    {
        In = 1,           // Stock In
        Out = 2,          // Stock Out
        Transfer = 3,     // Transfer between warehouses
        Adjustment = 4,   // Stock adjustment
        Return = 5,       // Return from customer
        Damage = 6,       // Damaged goods
        Expired = 7,      // Expired goods
        Reserved = 8,     // Reserved stock
        Unreserved = 9    // Unreserved stock
    }
}
