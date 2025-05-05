using ExamensArbete_BA_WIN23.Context;
using ExamensArbete_BA_WIN23.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose()
        => _context.Dispose();
}