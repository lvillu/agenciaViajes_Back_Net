using Domain.DataAccess;
using Domain.Entities;
using Infrastructure.Auth.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Auth.UnitOfWork
{
  public class AuthUnitOfWork : IAuthUnitOfWork
  {
    private readonly IRepository<User> _repository;

    public AuthUnitOfWork(IRepository<User> repository)
    {
      _repository = repository;
    }
    public IRepository<User> UserRepository => _repository;
  }
}
