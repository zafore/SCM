using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Attachment")]
    public class Attachment
    {
        [Key]
        [Column("AttachmentId")]
        public int AttachmentId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("AttachmentForTable")]
        public string AttachmentForTable { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("AttachmentName")]
        public string AttachmentName { get; set; } = string.Empty;

        [Column("AttachmentDate")]
        public DateTime? AttachmentDate { get; set; }

        // Navigation properties
        public virtual ICollection<AttachmentFile> AttachmentFiles { get; set; } = new List<AttachmentFile>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
        public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
        public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
