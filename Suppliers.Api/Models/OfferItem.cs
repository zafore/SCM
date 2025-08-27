using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("OfferItems")]
    public class OfferItem
    {
        [Key]
        [Column("OfferItemId")]
        public int OfferItemId { get; set; }

        [Required]
        [Column("OfferID")]
        public int OfferID { get; set; }

        [Required]
        [Column("ItemId")]
        public int ItemId { get; set; }

        [Column("ItemQuantity", TypeName = "decimal(18,2)")]
        public decimal? ItemQuantity { get; set; }

        [Column("ItemUnitPrice", TypeName = "decimal(18,2)")]
        public decimal? ItemUnitPrice { get; set; }

        [Column("CurrencyId")]
        public int? CurrencyId { get; set; }

        [Column("PrmotionFileAttachmentId")]
        public int? PrmotionFileAttachmentId { get; set; }

        // Navigation properties
        [ForeignKey("OfferID")]
        public virtual Offer Offer { get; set; } = null!;

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; }

        [ForeignKey("PrmotionFileAttachmentId")]
        public virtual Attachment? PrmotionFileAttachment { get; set; }
    }
}
