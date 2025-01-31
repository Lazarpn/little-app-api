using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.UserTask;

public class TaskCreateModel
{
    [Required(ErrorMessage = "Text is required.")]
    [StringLength(1000, ErrorMessage = "Text can't be more than 1000 characters.")]
    public string Text { get; set; }
}
