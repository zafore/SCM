using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class RoleType
{
    public int RoleTypeId { get; set; }

    public string RoleTypeName { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
