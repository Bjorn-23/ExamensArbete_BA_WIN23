using ExamensArbete_BA_WIN23.Business.Entities;
using ExamensArbete_BA_WIN23.Persistence;
using ExamensArbete_BA_WIN23.Repositories;
using ExamensArbete_BA_WIN23.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Services;

public class ChangeRequestService
{
    private readonly ILogger<ChangeRequestService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ChangeRequestRepo _changeRequestRepo;
    private readonly DateTimeProvider _dateTimeProvider;

    public ChangeRequestService(
        ILogger<ChangeRequestService> logger,
        IUnitOfWork unitOfWork,
        ChangeRequestRepo changeRequestRepo,
        DateTimeProvider dateTimeProvider
        )
    {
        _dateTimeProvider = dateTimeProvider;
        _changeRequestRepo = changeRequestRepo;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<IEnumerable<ChangeRequest>> GetAllAsync(CancellationToken ct)
    {
        var result = await _changeRequestRepo.Query().ToListAsync(ct);
        return result;
    }

    public async Task<IEnumerable<ChangeRequest>> GetExpiredChangeRequest(TimeSpan expirationTreshold, CancellationToken ct)
    {
        var expirationDate = _dateTimeProvider.UtcNow.Add(expirationTreshold);
        var result = await _changeRequestRepo.Query()
                    .Where(x => !x.isSignCompleted)
                    .Where(x => (x.Updated == null && x.Created <= expirationDate) || x.Created <= expirationDate)
                    .Where(x => x.DateSentBV == null)
                    .ToListAsync(ct);
        return result;
    }

    public async Task DeleteChangeRequest(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct)
    {
        foreach (var changeRequest in changeRequests)
        {
            _changeRequestRepo.Delete(changeRequest);
            _logger.LogInformation("Queued ChangeRequestID: {ChangeRequestID} for deletion", changeRequest.ChangeRequestId);
        }

        await _unitOfWork.SaveChangesAsync(ct);        
        _logger.LogInformation("Succesfully deleted {Count} ChangeRequests", changeRequests.Count());
    }

    public async Task NotifyRequestPendingDeletion(ChangeRequest changeRequest, CancellationToken ct)
    {
        // By using region and customer, get the relevant board members to notify about pending deletion.
        // Create new notification with: region, customer, IEnumerable<Guid>, NotificationEnum.
        // await _notificationRepo.SendNotification(changeRequest.Region, changeRequest.Customer, IEnumerable<Guid>, NotificationEnum, ct);
        _logger.LogInformation("Board members of Customer: {Customer} in Region: {Region}, were sent notifications for ChangeRequestID: {ChangeRequestID}", changeRequest.Customer, changeRequest.Region, changeRequest.ChangeRequestId);
    }
}
