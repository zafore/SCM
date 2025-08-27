using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMicroservice.Models
{
    [Table("StockAlerts")]
    public class StockAlert
    {
        [Key]
        [Column("AlertId")]
        public int AlertId { get; set; }

        [Required]
        [Column("InventoryItemId")]
        public int InventoryItemId { get; set; }

        [Required]
        [Column("AlertType")]
        public AlertType AlertType { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Message")]
        public string Message { get; set; } = string.Empty;

        [Column("CurrentStock", TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; }

        [Column("ThresholdValue", TypeName = "decimal(18,2)")]
        public decimal? ThresholdValue { get; set; }

        [Column("AlertDate")]
        public DateTime AlertDate { get; set; } = DateTime.UtcNow;

        [Column("IsRead")]
        public bool IsRead { get; set; } = false;

        [Column("IsResolved")]
        public bool IsResolved { get; set; } = false;

        [Column("ResolvedDate")]
        public DateTime? ResolvedDate { get; set; }

        [Column("ResolvedBy")]
        public int? ResolvedBy { get; set; }

        [MaxLength(200)]
        [Column("ResolutionNotes")]
        public string? ResolutionNotes { get; set; }

        // Navigation properties
        [ForeignKey("InventoryItemId")]
        public virtual InventoryItem InventoryItem { get; set; } = null!;
    }

    public enum AlertType
    {
        LowStock = 1,
        OutOfStock = 2,
        Overstock = 3,
        ExpiringSoon = 4,
        Expired = 5,
        NegativeStock = 6
    }
}
