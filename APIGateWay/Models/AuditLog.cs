using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGateWay.Models
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? UserName { get; set; }

        [MaxLength(100)]
        public string? UserEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string Action { get; set; } = string.Empty; // LOGIN, LOGOUT, CREATE, UPDATE, DELETE, VIEW

        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty; // User, Supplier, Offer, etc.

        [MaxLength(50)]
        public string? EntityId { get; set; }

        [MaxLength(500)]
        public string? EntityName { get; set; }

        [Required]
        [MaxLength(10)]
        public string HttpMethod { get; set; } = string.Empty; // GET, POST, PUT, DELETE

        [Required]
        [MaxLength(500)]
        public string Endpoint { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Microservice { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        [MaxLength(50)]
        public string? StatusCode { get; set; }

        [MaxLength(1000)]
        public string? RequestData { get; set; }

        [MaxLength(1000)]
        public string? ResponseData { get; set; }

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        [MaxLength(1000)]
        public string? Changes { get; set; } // JSON of what changed

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? SessionId { get; set; }

        [MaxLength(100)]
        public string? Role { get; set; }

        public TimeSpan? Duration { get; set; } // Request duration

        [MaxLength(1000)]
        public string? AdditionalInfo { get; set; }
    }

    public class AuditLogRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public string? EntityName { get; set; }
        public string HttpMethod { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string? Microservice { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? StatusCode { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Changes { get; set; }
        public string? SessionId { get; set; }
        public string? Role { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
