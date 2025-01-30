using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Common.Models.User;

public class AuthResponseModel
{
    public string Token { get; set; }
    public List<string> Roles { get; set; }
}
