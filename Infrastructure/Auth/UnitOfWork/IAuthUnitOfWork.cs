using Domain.DataAccess;
using Domain.Entities;

namespace Infrastructure.Auth.UnitOfWork
{
  public interface IAuthUnitOfWork
  {
    IRepository<User> UserRepository { get; }
  }
}
