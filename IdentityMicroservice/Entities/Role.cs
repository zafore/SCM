using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int RoleTypeId { get; set; }

    public virtual RoleType RoleType { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
