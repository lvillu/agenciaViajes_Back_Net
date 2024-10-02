using Infrastructure.Users.Models.Dto;
using Infrastructure.Users.Services;
using Infrastructure.Users.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Users.UseCases
{
  public class UserUseCases : IUserUseCases
  {

    IUserService _userService {  get; }

    public UserUseCases(IUserService userService)
    {
      _userService = userService;
    }
    public async Task<UserResponse> CreateUser(UserRequest userRequest)
    {
      return await _userService.CreateUser(userRequest);
    }

    public async Task<UserResponse> DeactiveUser(int userId)
    {
      return await _userService.DeactiveUser(userId);
    }

    public async Task<UserResponse> GetUser(int userId)
    {
      return await _userService.GetUser(userId);
    }

    public async Task<UserListResponse> GetUsers()
    {
      return await _userService.GetUsers();
    }

    public async Task<UserResponse> UpdateUser(UserRequest userRequest)
    {
      return await _userService.UpdateUser(userRequest);
    }
  }
}
