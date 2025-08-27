using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("RoleType")]
    public class RoleType
    {
        [Key]
        [Column("RoleTypeId")]
        public int RoleTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("RoleTypeName")]
        public string RoleTypeName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
