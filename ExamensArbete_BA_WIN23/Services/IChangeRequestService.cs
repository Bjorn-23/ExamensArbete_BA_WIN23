using ExamensArbete_BA_WIN23.Business.Entities;

namespace ExamensArbete_BA_WIN23.Services
{
    public interface IChangeRequestService
    {
        Task DeactivateNotification(ChangeRequest changeRequest);
        Task<bool> Delete(Guid id, CancellationToken token);
        Task DeleteExpiredChangeRequests(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct);
        Task<bool> Exists(Guid id, CancellationToken token);
        Task<IEnumerable<ChangeRequest>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<ChangeRequest>> GetExpiredChangeRequest(TimeSpan expirationTreshold, CancellationToken ct);
        Task<ChangeRequest> GetOne(Guid id, CancellationToken token);
        Task NotifyRequestPendingDeletion(IEnumerable<ChangeRequest> changeRequests, CancellationToken ct);
    }
}