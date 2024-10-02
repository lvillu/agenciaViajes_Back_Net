using Aplication.Auth.UseCases;
using Azure.Core;
using Azure;
using Infrastructure.Auth.Models;
using Infrastructure.Auth.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints
{
  public static class LoginEndpoints
  {
    public static void MapLoginEndPoints( this WebApplication app) {

      app.MapPost("/login", async ( HttpRequest request, IAuthUseCases authUseCases, HttpResponse response) =>
        {
          string email = request.Headers["email"].FirstOrDefault() ?? string.Empty;
          string password = request.Headers["password"].FirstOrDefault() ?? string.Empty;

          if (email.IsNullOrEmpty() || password.IsNullOrEmpty())
            return Results.BadRequest("Las credenciales son obligatorias");


          LoginResponse loginResponse = await authUseCases.Login(email, password);

          if (!loginResponse.success)
            return Results.Unauthorized();


          response.Cookies.Append("jwt", loginResponse.data.token, new CookieOptions
          {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
          });

          response.Cookies.Append("refreshToken", loginResponse.data.refreshToken, new CookieOptions
          {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
          });
          return Results.Ok(loginResponse);
        });

      app.MapPost("/refresh", async (HttpRequest request, IAuthUseCases authUseCases, HttpResponse response) =>
      {
        // Obtener el token de acceso y refresh token desde las cookies
        var accessToken = request.Cookies["jwt"];
        var refreshToken = request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
          return Results.BadRequest("Invalid client request");

        LoginResponse loginResponse = await authUseCases.RefreshToken(accessToken, refreshToken);

        if (!loginResponse.success)
          return Results.Unauthorized();


        response.Cookies.Append("jwt", loginResponse.data.token, new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.Strict
        });

        response.Cookies.Append("refreshToken", loginResponse.data.refreshToken, new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.Strict
        });
        return Results.Ok(loginResponse);

      });
      
      app.MapPost("/logout", async (HttpRequest request, IAuthUseCases authUseCases, HttpResponse response) =>
      {

        LoginResponse loginResponse = await authUseCases.Logout(request.Cookies["jwt"] ?? "");

        if (loginResponse.success)
        {
          response.Cookies.Delete("jwt");
          response.Cookies.Delete("refreshToken");
        }

        return Results.Ok(new { message = "Logout exitoso." });

      });

      app.MapGet("/secure", [Authorize] () =>
      {
        return Results.Ok(new { data = "Este es un dato protegido." });
      });

    }


  }
}
