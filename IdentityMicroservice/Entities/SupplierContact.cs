using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class SupplierContact
{
    public int SuppliersContactId { get; set; }

    public string? PersonName { get; set; }

    public string? PersonPosition { get; set; }

    public string? PersonEmail { get; set; }

    public string? PersonPhone { get; set; }

    public string? PersonWhatsapp { get; set; }

    public int? SupplierId { get; set; }

    public string? PersonPhoneTwo { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
