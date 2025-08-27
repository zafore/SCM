using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("SupplierContact")]
    public class SupplierContact
    {
        [Key]
        [Column("SuppliersContactId")]
        public int SuppliersContactId { get; set; }

        [MaxLength(50)]
        [Column("PersonName")]
        public string? PersonName { get; set; }

        [MaxLength(50)]
        [Column("PersonPosition")]
        public string? PersonPosition { get; set; }

        [MaxLength(50)]
        [Column("PersonEmail")]
        public string? PersonEmail { get; set; }

        [MaxLength(50)]
        [Column("PersonPhone")]
        public string? PersonPhone { get; set; }

        [MaxLength(50)]
        [Column("PersonWhatsapp")]
        public string? PersonWhatsapp { get; set; }

        [Column("SupplierId")]
        public int? SupplierId { get; set; }

        [MaxLength(50)]
        [Column("PersonPhoneTwo")]
        public string? PersonPhoneTwo { get; set; }

        // Navigation properties
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }
    }
}
