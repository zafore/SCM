using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class AttachmentFile
{
    public int AttachmentFileId { get; set; }

    public int? AttachmentId { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentFilePath { get; set; }

    public DateTime? CrDate { get; set; }

    public virtual Attachment? Attachment { get; set; }
}
