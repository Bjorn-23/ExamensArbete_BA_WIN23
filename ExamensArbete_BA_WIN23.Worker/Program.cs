using ExamensArbete_BA_WIN23.Context;
using ExamensArbete_BA_WIN23.Persistence;
using ExamensArbete_BA_WIN23.Repositories;
using ExamensArbete_BA_WIN23.Services;
using ExamensArbete_BA_WIN23.Utilities;
using ExamensArbete_BA_WIN23.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CleanupWorker>();

builder.Services.AddDbContext<ApplicationContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<DateTimeProvider>();
builder.Services.AddScoped<ChangeRequestRepo>();
builder.Services.AddScoped<ChangeRequestService>();
builder.Services.AddScoped<DbSeed>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seeder = services.GetRequiredService<DbSeed>();
    await seeder.SeedAsync();
}

host.Run();
