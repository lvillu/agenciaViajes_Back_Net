using Infrastructure.Auth.Models;
using Infrastructure.Auth.Service;
using Infrastructure.Auth.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Auth.UseCases
{
  public class AuthUseCases : IAuthUseCases
  {

    IAuthService _authService { get; }

    public AuthUseCases(IAuthService authService)
    {
      _authService = authService;
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
      return await _authService.Login(email, password);
    }

    public async Task<LoginResponse> RefreshToken(string token, string refreshToken)
    {
      return await _authService.RefreshToken(token, refreshToken);
    }

    public async Task<LoginResponse> Logout(string token)
    {
      return await _authService.Logout(token);
    }
  }
}
