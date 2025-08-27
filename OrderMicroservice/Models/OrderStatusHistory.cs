using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMicroservice.Models
{
    [Table("OrderStatusHistories")]
    public class OrderStatusHistory
    {
        [Key]
        [Column("StatusHistoryId")]
        public int StatusHistoryId { get; set; }

        [Required]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [Column("PreviousStatus")]
        public OrderStatus PreviousStatus { get; set; }

        [Required]
        [Column("NewStatus")]
        public OrderStatus NewStatus { get; set; }

        [MaxLength(500)]
        [Column("Comments")]
        public string? Comments { get; set; }

        [Column("ChangedBy")]
        public int? ChangedBy { get; set; }

        [MaxLength(50)]
        [Column("ChangedByName")]
        public string? ChangedByName { get; set; }

        [Column("ChangedDate")]
        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}
