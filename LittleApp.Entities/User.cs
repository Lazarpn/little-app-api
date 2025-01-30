using LittleApp.Entities.Identity;
using LittleApp.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Entities;

public class User : IdentityUser<Guid>, IEntity
{
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string FileName { get; set; }

    [MaxLength(1000)]
    public string FileOriginalName { get; set; }

    [MaxLength(1000)]
    public string FileUrl { get; set; }

    [MaxLength(1000)]
    public string ThumbUrl { get; set; }

    [MaxLength(6)]
    public string EmailVerificationCode { get; set; }

    public DateTime? DateVerificationCodeSent { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<UserClaim> Claims { get; set; } = new HashSet<UserClaim>();
    public virtual ICollection<UserLogin> Logins { get; set; } = new HashSet<UserLogin>();
    public virtual ICollection<UserToken> Tokens { get; set; } = new HashSet<UserToken>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}
