using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Carriers")]
    public class Carrier
    {
        [Key]
        [Column("CarrierID")]
        public int CarrierID { get; set; }

        [MaxLength(100)]
        [Column("CarrierName")]
        public string? CarrierName { get; set; }

        [MaxLength(250)]
        [Column("ContactInfo")]
        public string? ContactInfo { get; set; }

        // Navigation properties
        public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
    }
}
