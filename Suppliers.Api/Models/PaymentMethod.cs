using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key]
        [Column("PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("PaymentMethodName")]
        public string PaymentMethodName { get; set; } = string.Empty;
    }
}
