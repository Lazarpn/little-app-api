using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Entities;

public class Memory
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}
