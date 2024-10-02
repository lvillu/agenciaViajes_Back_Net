using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth.Models
{
  public class LoginDto
  {
    public required string email { get; set; }
    public required string token { get; set; }
    public required string refreshToken { get; set; }
  }
}
