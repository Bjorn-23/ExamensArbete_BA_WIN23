using ExamensArbete_BA_WIN23.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExamensArbete_BA_WIN23.Repositories;
public abstract class Repo<T> where T : class
{
    private readonly ApplicationContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repo( ApplicationContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<bool> Exists(Expression<Func<T, bool>> filter, CancellationToken ct)
        => await _dbSet.AsNoTracking().AnyAsync(filter, ct);

    public IQueryable<T> Query()
        => _dbSet;
 
    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public virtual async Task AddRangeAsync(IEnumerable<T> entity, CancellationToken ct = default)
        => await _dbSet.AddRangeAsync(entity, ct);

    public virtual void Update(T entity)
        => _dbSet.Update(entity);

    public virtual void Delete(T entity)
        => _dbSet.Remove(entity);

    public virtual async Task ClearAllData(CancellationToken ct)
        => _dbSet.RemoveRange(await _dbSet.ToListAsync<T>(ct));
}
