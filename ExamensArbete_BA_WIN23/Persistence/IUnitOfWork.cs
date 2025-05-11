namespace ExamensArbete_BA_WIN23.API.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
