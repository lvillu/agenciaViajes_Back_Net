using Aplication.Auth.Services;
using Aplication.Auth.UnitOfWork;
using Aplication.Auth.UseCases;
using Aplication.Helpers;
using Aplication.Users.Services;
using Aplication.Users.UnitOfWork;
using Aplication.Users.UseCases;
using Domain;
using Domain.DataAccess;
using Infrastructure.Auth.Service;
using Infrastructure.Auth.UnitOfWork;
using Infrastructure.Auth.UseCases;
using Infrastructure.Data;
using Infrastructure.Shared;
using Infrastructure.Users.Services;
using Infrastructure.Users.UnitOfWork;
using Infrastructure.Users.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Presentation.Extensions
{
  public static class ServiceExtensions
  {
    public static void AddAplicationService(this IServiceCollection services)
    {
      services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

      #region Unit Of Work
      services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
      services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
      #endregion

      #region User Cases
      services.AddScoped<IUserUseCases, UserUseCases>();
      services.AddScoped<IAuthUseCases, AuthUseCases>();
      #endregion

      #region Services
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IAuthService, AuthService>();
      #endregion


      services.AddScoped<TransactionHelper>();
      services.AddScoped<TokenHelper>();

    }

    public static void AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SqlConnection"))
      );
    }


    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen(s =>
      {
        s.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Proyecto de BackEnd Agencia de Viajes (Portal Admin)",
          Version = "v1"
        });
      });
    }

    public static void AddJWTAuth(this IServiceCollection services, IConfiguration configuration)
    {

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.Events = new JwtBearerEvents
        {
          OnMessageReceived = context =>
          {
            if (context.Request.Cookies.ContainsKey("jwt"))
              context.Token = context.Request.Cookies["jwt"];

            return Task.CompletedTask;
          }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = configuration["Jwt:Issuer"],
          ValidAudience = configuration["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
      });

    }
  }
}
