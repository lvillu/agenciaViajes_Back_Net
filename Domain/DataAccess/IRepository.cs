using System.Linq.Expressions;

namespace Domain.DataAccess
{
  public interface IRepository<T> where T : class
  {
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> GetAll();
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    void Delete(T entity);
    Task<int> SaveChangesAsync();
  }
}
