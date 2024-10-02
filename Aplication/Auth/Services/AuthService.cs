using Aplication.Helpers;
using Aplication.Users.UnitOfWork;
using Domain.Entities;
using Infrastructure.Auth.Models;
using Infrastructure.Auth.Service;
using Infrastructure.Auth.UnitOfWork;
using Infrastructure.Shared;
using Infrastructure.Users.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Aplication.Auth.Services
{
  public class AuthService : IAuthService
  {

    public IAuthUnitOfWork AuthUnitOfWork { get; }
    private readonly TokenHelper _tokenHelper;
    private readonly TransactionHelper _transactionHelper;

    public AuthService(IAuthUnitOfWork authUnitOfWork, TokenHelper tokenHelper, TransactionHelper transactionHelper)
    {
      AuthUnitOfWork = authUnitOfWork;
      _tokenHelper = tokenHelper;
      _transactionHelper = transactionHelper;
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
      try
      {
        LoginResponse loginResponse = new LoginResponse();

        User? user = await AuthUnitOfWork.UserRepository.Find( e => e.Email == email ).AsNoTracking().FirstOrDefaultAsync();

        if (user == null)
        {
          loginResponse.success = false;
          loginResponse.message = "Usuario no encontrado.";
          return loginResponse;
        }

        //Validamos el password
        if (!PasswordHelper.VerifyPassword(password, user.Password))
        {
          loginResponse.success = false;
          loginResponse.message = "Password incorrecto.";
          return loginResponse;
        }


        LoginDto loginDto = new LoginDto
        {
          email = user.Email,
          token = _tokenHelper.GenerateJwtToken(user),
          refreshToken = _tokenHelper.GenerateRefreshToken()
        };

        loginResponse.data = loginDto;

        _transactionHelper.BeginTransaction();

        user.RefreshToken = loginDto.refreshToken;
        await AuthUnitOfWork.UserRepository.Update(user);

        _transactionHelper.CommitTransaction();

        return loginResponse;
      }
      catch 
      {
        throw new Exception("Error");
      }
    }

    public async Task<LoginResponse> RefreshToken(string token, string refreshToken)
    {
      try
      {
        LoginResponse loginResponse = new LoginResponse();

        ClaimsPrincipal principal = _tokenHelper.GetPrincipalFromExpiredToken(token);

        if (principal == null || principal.Identity?.IsAuthenticated == false)
        {
          loginResponse.success = false;
          loginResponse.message = "Invalid Token";

          return loginResponse;
        }

        // Extraer el NameIdentifier(ID del usuario) desde los claims

        var userIdClaim = principal.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
        {
          loginResponse.success = false;
          loginResponse.message = "User ID not found in token";

          return loginResponse;
        }

        var refreshTokenClaim = principal.Claims.FirstOrDefault(c => c.Type == "RefreshTokenExpiration");
        if (refreshTokenClaim is null || DateTime.Parse(refreshTokenClaim.Value) <= DateTime.Now)
        {
          loginResponse.success = false;
          loginResponse.message = "User ID not found in token";

          return loginResponse;
        }

        User ? user = await AuthUnitOfWork.UserRepository.Find(e => e.Id.ToString() == userIdClaim.Value).AsNoTracking().FirstOrDefaultAsync();
        if (user == null || user.RefreshToken != refreshToken)
        {
          loginResponse.success = false;
          loginResponse.message = "Usuario no encontrado.";
          return loginResponse;
        }

        LoginDto loginDto = new LoginDto
        {
          email = user.Email,
          token = _tokenHelper.GenerateJwtToken(user),
          refreshToken = _tokenHelper.GenerateRefreshToken()
        };

        loginResponse.data = loginDto;

        _transactionHelper.BeginTransaction();

        user.RefreshToken = loginDto.refreshToken;
        await AuthUnitOfWork.UserRepository.Update(user);

        _transactionHelper.CommitTransaction();

        return loginResponse;
      }
      catch
      {
        throw new Exception("Error");
      }
    }

    public async Task<LoginResponse> Logout(string token)
    {
      try
      {
        LoginResponse loginResponse = new LoginResponse();

        ClaimsPrincipal principal = _tokenHelper.GetPrincipalFromExpiredToken(token);

        if (principal == null || principal.Identity?.IsAuthenticated == false)
          return loginResponse;

        // Extraer el NameIdentifier(ID del usuario) desde los claims

        var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
          return loginResponse;


        User? user = await AuthUnitOfWork.UserRepository.Find(e => e.Id.ToString() == userIdClaim.Value).AsNoTracking().FirstOrDefaultAsync();
        if (user == null)
          return loginResponse;

        _transactionHelper.BeginTransaction();

        user.RefreshToken = "";
        await AuthUnitOfWork.UserRepository.Update(user);

        _transactionHelper.CommitTransaction();

        return loginResponse;
      }
      catch
      {
        throw new Exception("Error");
      }
    }
  }
}
