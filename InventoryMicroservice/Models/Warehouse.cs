using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMicroservice.Models
{
    [Table("Warehouses")]
    public class Warehouse
    {
        [Key]
        [Column("WarehouseId")]
        public int WarehouseId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("WarehouseName")]
        public string WarehouseName { get; set; } = string.Empty;

        [MaxLength(200)]
        [Column("Address")]
        public string? Address { get; set; }

        [MaxLength(50)]
        [Column("City")]
        public string? City { get; set; }

        [MaxLength(50)]
        [Column("Country")]
        public string? Country { get; set; }

        [MaxLength(20)]
        [Column("PostalCode")]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        [Column("ManagerName")]
        public string? ManagerName { get; set; }

        [MaxLength(50)]
        [Column("ManagerPhone")]
        public string? ManagerPhone { get; set; }

        [MaxLength(50)]
        [Column("ManagerEmail")]
        public string? ManagerEmail { get; set; }

        [Column("Capacity")]
        public decimal? Capacity { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
    }
}
