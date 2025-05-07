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
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeRequestService(
        ILogger<ChangeRequestService> logger,
        IUnitOfWork unitOfWork,
        IChangeRequestRepo changeRequestRepo,
        IDateTimeProvider dateTimeProvider
        )
    {
        _dateTimeProvider = dateTimeProvider;
        _changeRequestRepo = changeRequestRepo;
        _unitOfWork = unitOfWork;
        _logger = logger;
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
    public async Task<bool> Delete(Guid id, CancellationToken token)
    {
        var changeRequest = await _changeRequestRepo.Query().Where(x => x.ChangeRequestId == id).FirstOrDefaultAsync(token);
        if (changeRequest == null)
        {
            return false;
        }

        _changeRequestRepo.Delete(changeRequest);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 1)
        {
            await DeactivateNotification(changeRequest);
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
            await DeactivateNotification(changeRequest);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Succesfully deleted {Count} ChangeRequests", changeRequests.Count());
    }

    public async Task NotifyRequestPendingDeletion(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct)
    {
        foreach (var changeRequest in changeRequests)
        {
            // By using region and customer, get the relevant board members to notify about pending deletion.
            // Create new notification with: region, customer, IEnumerable<Guid>, NotificationEnum.
            // await _notificationRepo.SendNotification(changeRequest.Region, changeRequest.Customer, IEnumerable<Guid>, NotificationEnum, ct);
            _logger.LogInformation("Board members of Customer: {Customer} in Region: {Region}, were sent notifications for ChangeRequestID: {ChangeRequestID}", changeRequest.Customer, changeRequest.Region, changeRequest.ChangeRequestId);
        }
    }

    public async Task DeactivateNotification(ChangeRequest changeRequest)
    {
        _logger.LogInformation("Deactivated notifications regarding ChangeRequestID: {ChangeRequestID} for board members of Customer: {Customer} in Region: {Region}", changeRequest.ChangeRequestId, changeRequest.Customer, changeRequest.Region);
    }
}
