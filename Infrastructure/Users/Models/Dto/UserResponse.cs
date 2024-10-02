using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Users.Models.Dto
{
  public class UserResponse
  {
    public bool success { get; set; } = true;
    public UserInfo data { get; set; }
    public string message { get; set; }
  }
}
