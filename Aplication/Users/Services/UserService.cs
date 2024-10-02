using Aplication.Helpers;
using Domain.Entities;
using Infrastructure.Shared;
using Infrastructure.Users.Models.Dto;
using Infrastructure.Users.Services;
using Infrastructure.Users.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Aplication.Users.Services
{
  public class UserService : IUserService
  {

    public IUserUnitOfWork UserUnitOfWork { get; }
    private readonly TransactionHelper transactionHelper;


    public UserService(IUserUnitOfWork userUnitOfWork, TransactionHelper transactionHelper)
    {
      UserUnitOfWork = userUnitOfWork;
      this.transactionHelper = transactionHelper;
    }

    public async Task<UserResponse> CreateUser(UserRequest userRequest)
    {
      try
      {
        UserResponse userResponse = new UserResponse();

        UserInfo? response = await (from a in UserUnitOfWork.UserRepository.GetAll()
                                        where a.Name == userRequest.Name
                                        select new UserInfo 
                                        { 
                                          Id = a.Id, Active = a.Active
                                        }).FirstOrDefaultAsync();

        if (response != null)
        {
          userResponse.success = false;
          userResponse.message = "Ya existe el usuario.";
        }

        transactionHelper.BeginTransaction();

        await UserUnitOfWork.UserRepository.Add(new User
        {
          Id = 0, Name = userRequest.Name, Password= PasswordHelper.EncryptPassword(userRequest.Password), 
          Email = userRequest.Email, Active = userRequest.Active
        });

        transactionHelper.CommitTransaction();

        return userResponse;
      }
      catch (Exception ex)
      {
        transactionHelper.RollbackTransaction();
        throw new Exception("Error: {0}", ex);
      }
    }

    public async Task<UserResponse> DeactiveUser(int userId)
    {
      try
      {
        UserResponse userResponse = new UserResponse();
        userResponse.success = true;

        User? getUser = await UserUnitOfWork.UserRepository.GetByIdAsync(userId);

        if (getUser == null)
        {
          userResponse.success = false;
          userResponse.message = "Usuario no encontrado.";
          return userResponse;
        }

        getUser.Active = false;

        transactionHelper.BeginTransaction();

        await UserUnitOfWork.UserRepository.Update(getUser);

        transactionHelper.CommitTransaction();

        return userResponse;
      }
      catch
      {
        transactionHelper.RollbackTransaction();
        throw new Exception("Error");
      }
    }

    public async Task<UserResponse> GetUser(int userId)
    {
      try
      {
        UserResponse userResponse = new UserResponse();
        var user = await UserUnitOfWork.UserRepository.GetByIdAsync(userId);

        if (user != null)
        {
          userResponse.data = new UserInfo
          {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Active = user.Active
          };
          userResponse.success = true;

        }


        return userResponse;

      }
      catch
      {
        throw new Exception("Error");
      }
    }

    public async Task<UserListResponse> GetUsers()
    {
      try
      {
        UserListResponse response = new UserListResponse();

        var query = (from a in UserUnitOfWork.UserRepository.GetAll()
                     select new
                     {
                       a.Id,
                       a.Name,
                       a.Email,
                       a.Active
                     }).AsQueryable();


        List<UserInfo> users = await query.Select(
          x => new UserInfo
          {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Active = x.Active
          }).ToListAsync();

        response.data = users;
        response.success = true;
        
        return response;
      }
      catch
      {

        throw new Exception("Error");
      }
    }

    public async Task<UserResponse> UpdateUser(UserRequest userRequest)
    {
      try
      {
        UserResponse userResponse = new UserResponse();
        userResponse.success = true;

        var duplicateName = await(from a in UserUnitOfWork.UserRepository.GetAll()
                                   where a.Name == userRequest.Name
                                   select a).FirstOrDefaultAsync();

        if (duplicateName != null)
        {
          userResponse.success = false;
          userResponse.message = "Nombre duplicado.";
          return userResponse;
        }


        var getUser = await UserUnitOfWork.UserRepository.GetByIdAsync(userRequest.Id);

        if (getUser == null)
        {
          userResponse.success = false;
          userResponse.message = "Usuario no encontrado.";
          return userResponse;
        }

        transactionHelper.BeginTransaction();

        await UserUnitOfWork.UserRepository.Update(new User
        {
          Id = userRequest.Id,
          Name = userRequest.Name,
          Password = PasswordHelper.EncryptPassword(userRequest.Password),
          Email = userRequest.Email,
          Active = userRequest.Active
        });

        transactionHelper.CommitTransaction();

        return userResponse;
      }
      catch
      {
        transactionHelper.RollbackTransaction();
        throw new Exception("Error");
      }
    }
  }
}
