using ExamensArbete_BA_WIN23.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {

    }
    public DbSet<ChangeRequest> ChangeRequest { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
