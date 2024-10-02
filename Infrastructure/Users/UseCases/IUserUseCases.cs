using Infrastructure.Users.Models.Dto;

namespace Infrastructure.Users.UseCases
{
  public interface IUserUseCases
  {
    Task<UserListResponse> GetUsers();
    Task<UserResponse> GetUser(int userId);
    Task<UserResponse> CreateUser(UserRequest userRequest);
    Task<UserResponse> UpdateUser(UserRequest userRequest);
    Task<UserResponse> DeactiveUser(int userId);
  }
}
