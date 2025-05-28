using ExamensArbete_BA_WIN23.Repositories;

namespace ExamensArbete_BA_WIN23.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
