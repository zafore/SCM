using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMicroservice.Models
{
    [Table("InventoryItems")]
    public class InventoryItem
    {
        [Key]
        [Column("InventoryItemId")]
        public int InventoryItemId { get; set; }

        [Required]
        [Column("ItemId")]
        public int ItemId { get; set; }

        [Required]
        [Column("WarehouseId")]
        public int WarehouseId { get; set; }

        [Required]
        [Column("CurrentStock", TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; }

        [Column("ReservedStock", TypeName = "decimal(18,2)")]
        public decimal ReservedStock { get; set; } = 0;

        [Column("AvailableStock", TypeName = "decimal(18,2)")]
        public decimal AvailableStock { get; set; }

        [Column("ReorderPoint", TypeName = "decimal(18,2)")]
        public decimal? ReorderPoint { get; set; }

        [Column("MaxStock", TypeName = "decimal(18,2)")]
        public decimal? MaxStock { get; set; }

        [Column("UnitCost", TypeName = "decimal(18,2)")]
        public decimal? UnitCost { get; set; }

        [MaxLength(50)]
        [Column("Location")]
        public string? Location { get; set; }

        [MaxLength(50)]
        [Column("BinNumber")]
        public string? BinNumber { get; set; }

        [Column("LastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; } = null!;

        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
    }
}
