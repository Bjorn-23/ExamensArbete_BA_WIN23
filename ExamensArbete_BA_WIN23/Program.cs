using ExamensArbete_BA_WIN23.API.Persistence;
using ExamensArbete_BA_WIN23.API.Repositories;
using ExamensArbete_BA_WIN23.API.Services;
using ExamensArbete_BA_WIN23.Context;
using ExamensArbete_BA_WIN23.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IChangeRequestRepo, ChangeRequestRepo>();
builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
builder.Services.AddScoped<IChangeRequestService, ChangeRequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
