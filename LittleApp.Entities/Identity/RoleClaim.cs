using Microsoft.AspNetCore.Identity;
using System;

namespace LittleApp.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; }
}
