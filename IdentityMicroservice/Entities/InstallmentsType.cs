using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class InstallmentsType
{
    public int InstallmentsTypeId { get; set; }

    public string InstallmentsTypeName { get; set; } = null!;

    public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();

    public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
}
