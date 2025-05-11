using ExamensArbete_BA_WIN23.API.Entities;
using ExamensArbete_BA_WIN23.Context;

namespace ExamensArbete_BA_WIN23.API.Repositories;

public class NotificationRepo(ApplicationContext context) : Repo<ApplicationContext, Notification>(context), INotificationRepo
{
    private readonly ApplicationContext _context = context;
}