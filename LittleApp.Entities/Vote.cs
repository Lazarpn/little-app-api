using LittleApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Entities;

public class Vote
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Answer Answer { get; set; }

    [InverseProperty(nameof(Entities.User.Votes))]
    public virtual User User { get; set; }
}
