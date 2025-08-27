using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class OfferStatus
{
    public int OfferStatusId { get; set; }

    public string OfferStatusName { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
