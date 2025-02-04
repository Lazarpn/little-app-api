using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.Vote;

public class VotesMeModel
{
    public Guid UserId { get; set; }
    public List<VoteModel> Votes { get; set; }
}
