using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.User;

public class UserShortModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarThumbUrl { get; set; }

    public string Initials
    {
        get => InitialsHelper.Get(FirstName, LastName);
    }
}
