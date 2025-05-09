using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExamensArbete_BA_WIN23.Repositories;
public abstract partial class Repo<TContext, T> : IRepo<TContext, T> where TContext : DbContext where T :class
{
    private readonly TContext _dbContext;
    private readonly DbSet<T> _dbSet;

    protected Repo(TContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<bool> Exists(Expression<Func<T, bool>> filter, CancellationToken ct)
        => await _dbSet.AsNoTracking().AnyAsync(filter, ct);

    public virtual IQueryable<T> Query()
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
