using Infrastructure.Users.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Users.Services
{
  public interface IUserService
  {
    Task<UserListResponse> GetUsers();
    Task<UserResponse> GetUser(int userId);
    Task<UserResponse> CreateUser(UserRequest userRequest);
    Task<UserResponse> UpdateUser(UserRequest userRequest);
    Task<UserResponse> DeactiveUser( int userId );
  }
}
