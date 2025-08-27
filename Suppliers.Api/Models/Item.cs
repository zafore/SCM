using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Items")]
    public class Item
    {
        [Key]
        [Column("ItemId")]
        public int ItemId { get; set; }

        [MaxLength(50)]
        [Column("ItemName")]
        public string? ItemName { get; set; }

        [MaxLength(50)]
        [Column("ItemCode")]
        public string? ItemCode { get; set; }

        [MaxLength(50)]
        [Column("ItemBarCode")]
        public string? ItemBarCode { get; set; }

        // Navigation properties
        public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
    }
}
