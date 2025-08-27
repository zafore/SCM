using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? CountryFlag { get; set; }

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
