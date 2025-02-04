using LittleApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.Vote;

public class VoteModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Answer Answer { get; set; }
}
