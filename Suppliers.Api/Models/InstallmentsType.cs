using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("InstallmentsType")]
    public class InstallmentsType
    {
        [Key]
        [Column("InstallmentsTypeId")]
        public int InstallmentsTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("InstallmentsTypeName")]
        public string InstallmentsTypeName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();
        public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
    }
}
