using ExamensArbete_BA_WIN23.Business.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {

    }
    public DbSet<ChangeRequestDto> ChangeRequest { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Debug);
    }
}
