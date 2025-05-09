using ExamensArbete_BA_WIN23.API.Entities;
using ExamensArbete_BA_WIN23.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {

    }
    public DbSet<ChangeRequest> ChangeRequests { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Notification>()
        //    .HasOne(x => x.ChangeRequest)
        //    .WithOne()
        //    .HasForeignKey<ChangeRequest>(x => x.ChangeRequestId);
    }
}
