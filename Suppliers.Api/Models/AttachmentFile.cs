using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("AttachmentFiles")]
    public class AttachmentFile
    {
        [Key]
        [Column("AttachmentFileId")]
        public int AttachmentFileId { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        [MaxLength(50)]
        [Column("AttachmentFileName")]
        public string? AttachmentFileName { get; set; }

        [Column("AttachmentFilePath")]
        public string? AttachmentFilePath { get; set; }

        [Column("CrDate")]
        public DateTime? CrDate { get; set; }

        // Navigation properties
        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }
    }
}
