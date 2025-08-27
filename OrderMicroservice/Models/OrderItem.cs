using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMicroservice.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [Column("OrderItemId")]
        public int OrderItemId { get; set; }

        [Required]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [Column("ItemId")]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ItemName")]
        public string ItemName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("ItemCode")]
        public string? ItemCode { get; set; }

        [MaxLength(200)]
        [Column("Description")]
        public string? Description { get; set; }

        [Required]
        [Column("Quantity", TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column("UnitPrice", TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column("DiscountPercentage", TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column("DiscountAmount", TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("TaxRate", TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; }

        [Column("TaxAmount", TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column("LineTotal", TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        [Column("ShippedQuantity", TypeName = "decimal(18,2)")]
        public decimal ShippedQuantity { get; set; }

        [Column("ReceivedQuantity", TypeName = "decimal(18,2)")]
        public decimal ReceivedQuantity { get; set; }

        [Column("WarehouseId")]
        public int? WarehouseId { get; set; }

        [MaxLength(50)]
        [Column("Location")]
        public string? Location { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}
