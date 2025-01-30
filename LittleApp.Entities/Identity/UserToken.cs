using Microsoft.AspNetCore.Identity;
using System;

namespace LittleApp.Entities.Identity;

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; }
}
