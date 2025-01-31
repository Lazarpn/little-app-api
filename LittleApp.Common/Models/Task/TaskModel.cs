using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.UserTask;

public class TaskModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Order { get; set; }
    public DateTime? CompletedAt { get; set; }
}
