using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Currency")]
    public class Currency
    {
        [Key]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("CurrencyName")]
        public string CurrencyName { get; set; } = string.Empty;

        [Column("CurrencyFlag")]
        public string? CurrencyFlag { get; set; }

        // Navigation properties
        public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
        public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
        public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
    }
}
