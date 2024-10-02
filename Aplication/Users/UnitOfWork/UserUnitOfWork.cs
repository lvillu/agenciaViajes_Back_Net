using Domain.DataAccess;
using Domain.Entities;
using Infrastructure.Users.UnitOfWork;

namespace Aplication.Users.UnitOfWork
{
  public class UserUnitOfWork : IUserUnitOfWork
  {
    private readonly IRepository<User> _repository;

    public UserUnitOfWork(IRepository<User> repository)
    {
      _repository = repository;
    }

    public IRepository<User> UserRepository => _repository;
  }
}
