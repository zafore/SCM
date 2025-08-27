using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [Column("SupplierId")]
        public int SupplierId { get; set; }

        [MaxLength(50)]
        [Column("SupplierName")]
        public string? SupplierName { get; set; }

        [MaxLength(50)]
        [Column("Phone")]
        public string? Phone { get; set; }

        [MaxLength(50)]
        [Column("Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
        [Column("Address")]
        public string? Address { get; set; }

        [MaxLength(50)]
        [Column("Websit")]
        public string? Websit { get; set; }

        [Column("CountryId")]
        public int? CountryId { get; set; }

        [Column("IsActive")]
        public bool? IsActive { get; set; }

        [Column("SupplierNote")]
        public string? SupplierNote { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        // Navigation properties
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
    }
}
