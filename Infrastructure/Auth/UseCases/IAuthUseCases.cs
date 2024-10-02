using Infrastructure.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth.UseCases
{
  public interface IAuthUseCases
  {
    Task<LoginResponse> Login(string email, string password);
    Task<LoginResponse> RefreshToken(string token, string refreshToken);
    Task<LoginResponse> Logout(string token);
  }
}
