using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        [Column("CountryId")]
        public int CountryId { get; set; }

        [MaxLength(50)]
        [Column("CountryName")]
        public string? CountryName { get; set; }

        [Column("CountryFlag")]
        public string? CountryFlag { get; set; }

        // Navigation properties
        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
