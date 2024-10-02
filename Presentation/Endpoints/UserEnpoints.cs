using Infrastructure.Users.Models.Dto;
using Infrastructure.Users.UseCases;

namespace Presentation.Endpoints
{
  public static class UserEnpoints
  {
    public static void MapUsersEndpoints(this WebApplication app)
    {
      const string baseAPIUrl = "api/users";

      app.MapGet(baseAPIUrl + "/users", async (IUserUseCases userUseCases) =>
      {
        UserListResponse users = await userUseCases.GetUsers();

        if (users.success)
          return Results.Ok(users.data);
        else
          return Results.BadRequest(users.message);
      });

      app.MapGet(baseAPIUrl + "/user/{id:int}", async (int id, IUserUseCases userUseCases) =>
      {
        UserResponse user = await userUseCases.GetUser(id);

        if(user != null)
          return Results.NotFound();

        if (user.success)
          return Results.Ok(user.data);
        else
          return Results.BadRequest(user.message);
      });

      app.MapPost(baseAPIUrl + "/user/", async (UserRequest userRequest, IUserUseCases userUseCases) =>
      {
        UserResponse user =  await userUseCases.CreateUser(userRequest);

        if (user.success)
          return Results.Created("user", user.data);
        else
          return Results.BadRequest(user.message);

      });

      app.MapPut(baseAPIUrl + "/user/{id:int}", async (int id, UserRequest userRequest, IUserUseCases userUseCases) =>
      {
        UserResponse user = await userUseCases.UpdateUser(userRequest);

        if (user.success)
          return Results.NoContent();
        else
          return Results.BadRequest(user.message);

      });

      app.MapPut(baseAPIUrl + "/deactive/{id:int}", async (int id, IUserUseCases userUseCases) =>
      {
        UserResponse user = await userUseCases.DeactiveUser(id);

        if (user.success)
          return Results.NoContent();
        else
          return Results.BadRequest(user.message);

      });

    }
  }
}
