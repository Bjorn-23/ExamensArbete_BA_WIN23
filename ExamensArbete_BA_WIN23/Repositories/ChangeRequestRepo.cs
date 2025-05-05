using ExamensArbete_BA_WIN23.Business.Dtos;
using ExamensArbete_BA_WIN23.Context;

namespace ExamensArbete_BA_WIN23.Repositories;

public class ChangeRequestRepo : Repo<ChangeRequestDto>
{
    public ChangeRequestRepo(ApplicationContext dbContext) : base(dbContext)
    {
    }
}
