using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.UserTask;

public class TaskReorderModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Order number must be greater than or equal to one.")]
    public int Order { get; set; }

    public DateTime? CompletedAt { get; set; }
}
