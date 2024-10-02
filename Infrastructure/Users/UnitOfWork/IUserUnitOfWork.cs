using Domain.DataAccess;
using Domain.Entities;

namespace Infrastructure.Users.UnitOfWork
{
  public interface IUserUnitOfWork
  {
    IRepository<User> UserRepository { get; }
  }
}
