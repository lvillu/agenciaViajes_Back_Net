using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth.Models
{
  public class LoginResponse
  {
        public string message { get; set; } = string.Empty;
        public LoginDto data { get; set; }
        public bool success { get; set; } = true;
    }
}
