using ExamensArbete_BA_WIN23.API.Entities;
using ExamensArbete_BA_WIN23.Business.Entities;
using ExamensArbete_BA_WIN23.Persistence;
using ExamensArbete_BA_WIN23.Repositories;
using ExamensArbete_BA_WIN23.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ExamensArbete_BA_WIN23.Services;

public class ChangeRequestService : IChangeRequestService
{
    private readonly ILogger<ChangeRequestService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChangeRequestRepo _changeRequestRepo;
    private readonly INotificationRepo _notificationRepo;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeRequestService(
        ILogger<ChangeRequestService> logger,
        IUnitOfWork unitOfWork,
        IChangeRequestRepo changeRequestRepo,
        INotificationRepo notificationRepo,
        IDateTimeProvider dateTimeProvider
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _changeRequestRepo = changeRequestRepo;
        _notificationRepo = notificationRepo;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Exists(Guid id, CancellationToken token)
    {
        var changeRequest = await _changeRequestRepo.Query().AsNoTracking().Where(x => x.ChangeRequestId == id).FirstOrDefaultAsync(token);

        return true;
    }

    public async Task<ChangeRequest> GetOne(Guid id, CancellationToken token)
    {
        var result = await _changeRequestRepo.Query().Where(x => x.ChangeRequestId == id).FirstOrDefaultAsync(token);
        return result!;
    }

    public async Task<IEnumerable<ChangeRequest>> GetAllAsync(CancellationToken ct)
    {
        var result = await _changeRequestRepo.Query().ToListAsync();
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
    public async Task<bool> Delete(Guid id, CancellationToken ct)
    {
        var changeRequest = await _changeRequestRepo.Query().Where(x => x.ChangeRequestId == id).FirstOrDefaultAsync(ct);
        if (changeRequest == null)
        {
            return false;
        }

        _changeRequestRepo.Delete(changeRequest);
        await DeactivateNotification(changeRequest, ct);

        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 1)
        {
            return true;
        }

        return false;
    }

    public async Task DeleteExpiredChangeRequests(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct)
    {
        foreach (var changeRequest in changeRequests)
        {
            _changeRequestRepo.Delete(changeRequest);
            _logger.LogInformation("Queued ChangeRequestID: {ChangeRequestID} for deletion", changeRequest.ChangeRequestId);
            await DeactivateNotification(changeRequest, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Succesfully deleted {Count} ChangeRequests", changeRequests.Count());
    }

    public async Task NotifyRequestsPendingDeletion(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct)
    {
        foreach (var changeRequest in changeRequests)
        {
            await NotifyRequestPendingDeletion(changeRequest, ct);
        }
        await _unitOfWork.SaveChangesAsync(ct);
    }

    private async Task NotifyRequestPendingDeletion(ChangeRequest changeRequest, CancellationToken ct)
    {

        if (await _notificationRepo.Exists(x => x.ChangeRequestId == changeRequest.ChangeRequestId && !x.isDeactivated, ct))
        {
            return;
        }

        var notification = new Notification()
        {
            Type = NotificationType.changeRequest,
            ChangeRequestId = changeRequest.ChangeRequestId,
            Created = _dateTimeProvider.UtcNow,
            isDeactivated = false,
            Message = $"You have an unhandled changeRequest: {changeRequest.ChangeRequestId}, if no action is taken it will be deleted.",
        };

        await _notificationRepo.AddAsync(notification, ct);
        _logger.LogInformation("Board members of Customer: {Customer} in Region: {Region}, were sent notifications for ChangeRequestID: {ChangeRequestID}", changeRequest.Customer, changeRequest.Region, changeRequest.ChangeRequestId);
    }

    public async Task DeactivateNotification(ChangeRequest changeRequest, CancellationToken ct)
    {
        var notification = await _notificationRepo.Query().Where(x => x.ChangeRequestId == changeRequest.ChangeRequestId).FirstOrDefaultAsync(ct);
        if (notification == null)
        {
            return;
        }

        _notificationRepo.Delete(notification);
        _logger.LogInformation("Deactivated notifications regarding ChangeRequestID: {ChangeRequestID} for board members of Customer: {Customer} in Region: {Region}", changeRequest.ChangeRequestId, changeRequest.Customer, changeRequest.Region);
    }
}
