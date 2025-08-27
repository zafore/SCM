using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("RoleId")]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("RoleName")]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [Column("RoleTypeId")]
        public int RoleTypeId { get; set; }

        // Navigation properties
        [ForeignKey("RoleTypeId")]
        public virtual RoleType RoleType { get; set; } = null!;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
