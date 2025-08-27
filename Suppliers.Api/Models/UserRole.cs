using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        [Column("UserRoleId")]
        public int UserRoleId { get; set; }

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [Column("RoleId")]
        public int RoleId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
    }
}
