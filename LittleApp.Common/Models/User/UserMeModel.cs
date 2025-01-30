using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.User;

public class UserMeModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarThumbUrl { get; set; }
    public bool EmailConfirmed { get; set; }

    public string Initials
    {
        get => InitialsHelper.Get(FirstName, LastName);
    }

    public DateTime DateVerificationCodeExpires { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class InitialsHelper
{
    public static string Get(string firstName, string lastName)
    {
        var result = string.Empty;
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            result += firstName[0];
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            result += lastName[0];
        }
        else if (firstName?.Length > 1)
        {
            result += firstName[1];
        }

        return result.ToUpper();
    }
}
