using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Websit { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public string? SupplierNote { get; set; }

    public int? AttachmentId { get; set; }

    public virtual Attachment? Attachment { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
}
