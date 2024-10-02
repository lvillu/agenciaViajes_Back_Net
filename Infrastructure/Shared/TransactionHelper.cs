using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Shared
{
  public class TransactionHelper
  {
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public TransactionHelper(ApplicationDbContext context)
    {
      _context = context;
      _transaction = null;
    }

    public void BeginTransaction()
    {
      _transaction = _context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
      _transaction?.Commit();
    }

    public void SaveAndCommit()
    {
      _context.SaveChanges();
      _transaction?.Commit();
    }

    public void RollbackTransaction()
    {
      _transaction?.Rollback();
    }

    public void Savechanges()
    {
      _context.SaveChanges();
    }

    public void Dispose() {
      _transaction?.Dispose();
    }

  }
}
