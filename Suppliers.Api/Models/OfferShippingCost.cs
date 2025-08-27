using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("OfferShippingCost")]
    public class OfferShippingCost
    {
        [Key]
        [Column("OfferShippingCostId")]
        public int OfferShippingCostId { get; set; }

        [Required]
        [Column("CarrierID")]
        public int CarrierID { get; set; }

        [Required]
        [Column("ShipmentTypeId")]
        public int ShipmentTypeId { get; set; }

        [Required]
        [Column("ShippingCost", TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("ShippingCostNote")]
        public string? ShippingCostNote { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        [Required]
        [Column("OfferID")]
        public int OfferID { get; set; }

        // Navigation properties
        [ForeignKey("CarrierID")]
        public virtual Carrier Carrier { get; set; } = null!;

        [ForeignKey("ShipmentTypeId")]
        public virtual ShipmentType ShipmentType { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; } = null!;

        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        [ForeignKey("OfferID")]
        public virtual Offer Offer { get; set; } = null!;

        public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
    }
}
