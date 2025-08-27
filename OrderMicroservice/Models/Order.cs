using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMicroservice.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("OrderNumber")]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [Column("OrderType")]
        public OrderType OrderType { get; set; }

        [Required]
        [Column("CustomerId")]
        public int CustomerId { get; set; }

        [Column("SupplierId")]
        public int? SupplierId { get; set; }

        [Column("OfferId")]
        public int? OfferId { get; set; }

        [Required]
        [Column("OrderDate")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column("RequiredDate")]
        public DateTime? RequiredDate { get; set; }

        [Column("ShippedDate")]
        public DateTime? ShippedDate { get; set; }

        [Column("DeliveredDate")]
        public DateTime? DeliveredDate { get; set; }

        [Required]
        [Column("OrderStatus")]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [Column("SubTotal", TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column("TaxAmount", TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column("ShippingCost", TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column("DiscountAmount", TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("TotalAmount", TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(200)]
        [Column("ShippingAddress")]
        public string? ShippingAddress { get; set; }

        [MaxLength(200)]
        [Column("BillingAddress")]
        public string? BillingAddress { get; set; }

        [MaxLength(50)]
        [Column("PaymentMethod")]
        public string? PaymentMethod { get; set; }

        [Column("PaymentStatus")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [MaxLength(500)]
        [Column("Notes")]
        public string? Notes { get; set; }

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }

        [MaxLength(50)]
        [Column("CreatedByName")]
        public string? CreatedByName { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();
    }

    public enum OrderType
    {
        Purchase = 1,    // Purchase Order (from supplier)
        Sales = 2,       // Sales Order (to customer)
        Transfer = 3,    // Transfer Order (between warehouses)
        Return = 4       // Return Order
    }

    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6,
        Returned = 7,
        Completed = 8
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        PartiallyPaid = 3,
        Failed = 4,
        Refunded = 5
    }
}
