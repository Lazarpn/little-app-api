using Microsoft.AspNetCore.Identity;
using System;

namespace LittleApp.Entities.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; }
}
