using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("OfferStatus")]
    public class OfferStatus
    {
        [Key]
        [Column("OfferStatusId")]
        public int OfferStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("OfferStatusName")]
        public string OfferStatusName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
