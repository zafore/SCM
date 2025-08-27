using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("ShipmentType")]
    public class ShipmentType
    {
        [Key]
        [Column("ShipmentTypeId")]
        public int ShipmentTypeId { get; set; }

        [MaxLength(50)]
        [Column("ShipmentTypeName")]
        public string? ShipmentTypeName { get; set; }

        // Navigation properties
        public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
    }
}
