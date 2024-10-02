using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Users.Models.Dto
{
  public class UserListResponse
  {
    public bool success {  get; set; }
    public List<UserInfo> data { get; set; }
    public string message { get; set; }
  }
}
