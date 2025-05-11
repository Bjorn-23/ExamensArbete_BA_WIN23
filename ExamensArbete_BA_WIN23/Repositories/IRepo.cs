using System.Linq.Expressions;

namespace ExamensArbete_BA_WIN23.API.Repositories;

public interface IRepo<TContext, T> where TContext : class where T : class
{
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entity, CancellationToken ct = default);
    Task ClearAllData(CancellationToken ct);
    void Delete(T entity);
    Task<bool> Exists(Expression<Func<T, bool>> filter, CancellationToken ct);
    IQueryable<T> Query();
    void Update(T entity);
}