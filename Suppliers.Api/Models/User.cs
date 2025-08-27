using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("PassWord")]
        public string PassWord { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("FullName")]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
        [Column("Phone")]
        public string? Phone { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
