using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("PaymentStates")]
    public class PaymentState
    {
        [Key]
        [Column("PaymentStatesId")]
        public int PaymentStatesId { get; set; }

        [Required]
        [Column("PaymentStatesName")]
        public string PaymentStatesName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();
        public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
    }
}
