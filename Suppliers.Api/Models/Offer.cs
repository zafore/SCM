using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Offer")]
    public class Offer
    {
        [Key]
        [Column("OfferID")]
        public int OfferID { get; set; }
    
        [Column("OfferName")]
        public string? OfferName { get; set; }

        [Column("OfferDescription")]
        public string? OfferDescription { get; set; }

        [Column("OfferDate")]
        public DateTime? OfferDate { get; set; }
        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }
        [Column("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }
        
        [Column("OfferStatusId")]
        public int? OfferStatusId { get; set; }

        [Column("SupplierID")]
        public int? SupplierID { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        // Navigation properties
        [ForeignKey("OfferStatusId")]
        public virtual OfferStatus? OfferStatus { get; set; }

        [ForeignKey("SupplierID")]
        public virtual Supplier? Supplier { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
        public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
    }
}
