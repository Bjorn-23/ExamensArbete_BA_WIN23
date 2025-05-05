
using ExamensArbete_BA_WIN23.Business.Dtos;
using ExamensArbete_BA_WIN23.Context;
using ExamensArbete_BA_WIN23.Persistence;
using ExamensArbete_BA_WIN23.Repositories;

namespace ExamensArbete_BA_WIN23.Worker;
public class DbSeed
{
    private readonly ChangeRequestRepo _changeRequestRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DbSeed(ChangeRequestRepo changeRequestRepo, IUnitOfWork unitOfWork)
    {
        _changeRequestRepo = changeRequestRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var changeRequests = new[]
        {
            new ChangeRequestDto()
            {
                Created = DateTime.Now.AddDays(-120),
                Updated = null,
                Customer = 1,
                Region = 1,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = null,
                isSignCompleted = false,
                DateSentBV = null,
            },
            new ChangeRequestDto()
            {
                Customer = 2,
                Region = 1,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = Guid.NewGuid(),
                isSignCompleted = true,
                DateSentBV = DateTimeOffset.UtcNow.AddDays(-60),
            },
            new ChangeRequestDto()
            {
                Created = DateTime.Now.AddDays(-65),
                Customer = 1,
                Region = 2,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = null!,
                isSignCompleted = false,
                DateSentBV = null!,
            },
            new ChangeRequestDto()
            {
                Customer = 2,
                Region = 2,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = null!,
                isSignCompleted = false,
                DateSentBV = null!,
            }

        };

        await _changeRequestRepo.AddRangeAsync(changeRequests);
        await _unitOfWork.SaveChangesAsync();
    }
}
