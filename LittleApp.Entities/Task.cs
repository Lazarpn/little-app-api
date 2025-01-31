using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleApp.Entities.Interfaces;

namespace LittleApp.Entities;

public class Task : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Text { get; set; }

    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    [InverseProperty(nameof(Entities.User.Tasks))]
    public virtual User User { get; set; }
}
