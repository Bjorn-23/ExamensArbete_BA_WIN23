using ExamensArbete_BA_WIN23.API.Entities;
using ExamensArbete_BA_WIN23.API.Persistence;
using ExamensArbete_BA_WIN23.API.Repositories;
using ExamensArbete_BA_WIN23.Utilities;

namespace ExamensArbete_BA_WIN23.Worker;
public class DbSeed
{
    private readonly ILogger<DbSeed> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChangeRequestRepo _changeRequestRepo;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DbSeed(
        ILogger<DbSeed> logger,
        IUnitOfWork unitOfWork,
        IChangeRequestRepo changeRequestRepo,
        IDateTimeProvider dateTimeProvider
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _changeRequestRepo = changeRequestRepo;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task SeedAsync()
    {
        var changeRequests = new[]
        {
            new ChangeRequest()
            {
                // Detta objekt ska tas bort från databasen
                ChangeRequestId = Guid.Parse("9f72e9ec-2e53-4c87-90be-913e2e6f7a78"),
                Created = _dateTimeProvider.UtcNow.AddDays(-120),
                Updated = null,
                Customer = 1,
                Region = 1,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = null,
                isSignCompleted = false,
                DateSentBV = null,
            },
            new ChangeRequest()
            {
                // Detta objekt ska inte hanteras då det skickats till bolagsverket och isSignCompleted == true
                ChangeRequestId = Guid.Parse("2a1a2f14-157d-4660-9324-5105efc70149"),
                Created = _dateTimeProvider.UtcNow.AddDays(-120),
                Updated = _dateTimeProvider.UtcNow.AddDays(-65),
                Customer = 2,
                Region = 1,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = Guid.NewGuid(),
                isSignCompleted = true,
                DateSentBV = DateTimeOffset.UtcNow.AddDays(-60),
            },
            new ChangeRequest()
            {
                // Detta objekt ska hanteras och en notis ska skapas för: Customer 1, region 2
                ChangeRequestId = Guid.Parse("f7f36e9c-d15b-46f6-8754-c847a8a2efb3"),
                Created = _dateTimeProvider.UtcNow.AddDays(-65),
                Updated = _dateTimeProvider.UtcNow.AddDays(-62),
                Customer = 1,
                Region = 2,
                FirstApprover = Guid.NewGuid(),
                SecondApprover = null!,
                isSignCompleted = false,
                DateSentBV = null!,
            },
            new ChangeRequest()
            {
                // Detta objekt ska inte hanteras då det bara är 15 minuter gammalt
                ChangeRequestId = Guid.Parse("4e703c1c-bc33-4a8f-8bd5-1b99cc1b144e"),
                Created = _dateTimeProvider.UtcNow.AddMinutes(-15),
                Updated = null,
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
        _logger.LogInformation("Seeded database with {Count} entries for demo", changeRequests.Count());
    }

    public async Task RemoveSeed(CancellationToken ct)
    {
        await _changeRequestRepo.ClearAllData(ct: ct);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Cleared database for demo");
    }
}
