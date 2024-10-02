using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Domain.DataAccess
{
  public class Repository<T> : IRepository<T> where T : class
  {

    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
      _context = context;
      _dbSet = context.Set<T>();
    }
    public async Task<T> Add(T entity)
    {
      await _dbSet.AddAsync(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async void Delete(T entity)
    {
      _dbSet.Remove(entity);
      await _context.SaveChangesAsync();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
      return _dbSet.Where(predicate);
    }

    public IQueryable<T> GetAll()
    {
      return _dbSet;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
      return await _dbSet.FindAsync(id);
    }

    public Task<int> SaveChangesAsync()
    {
      return _context.SaveChangesAsync();
    }

    public async Task<T> Update(T entity)
    {
      _dbSet.Update(entity);
      await _context.SaveChangesAsync();
      return entity;
    }
  }
}
