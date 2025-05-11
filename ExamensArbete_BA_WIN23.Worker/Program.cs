using ExamensArbete_BA_WIN23.API.Persistence;
using ExamensArbete_BA_WIN23.API.Repositories;
using ExamensArbete_BA_WIN23.API.Services;
using ExamensArbete_BA_WIN23.Context;
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
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IChangeRequestRepo, ChangeRequestRepo>();
builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
builder.Services.AddScoped<IChangeRequestService, ChangeRequestService>();
builder.Services.AddScoped<DbSeed>();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = null;
    options.IncludeScopes = false;
});

var host = builder.Build();
host.Run();
