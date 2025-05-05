using ExamensArbete_BA_WIN23.Utilities;
using ExamensArbete_BA_WIN23.Business.Dtos;
using ExamensArbete_BA_WIN23.Repositories;
using Microsoft.EntityFrameworkCore;
using ExamensArbete_BA_WIN23.Persistence;

namespace ExamensArbete_BA_WIN23.Services;

public class ChangeRequestService
{
    private readonly DateTimeProvider _dateTimeProvider;
    private readonly ChangeRequestRepo _changeRequestRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRequestService(
        DateTimeProvider dateTimeProvider,
        ChangeRequestRepo changeRequestRepo,
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _changeRequestRepo = changeRequestRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ChangeRequestDto>> GetExpiredChangeRequest(TimeSpan expirationTreshold, CancellationToken ct)
    {
        var expirationDate = _dateTimeProvider.UtcNow.Add(expirationTreshold);
        var result = await _changeRequestRepo.Query()
                    .Where(x => !x.isSignCompleted)
                    .Where(x => (x.Updated == null && x.Created <= expirationDate) || x.Created <= expirationDate)
                    .Where(x => x.DateSentBV == null)
                    .ToListAsync(ct);
        return result;
    }

    public async Task DeleteChangeRequest(ChangeRequestDto dto, CancellationToken ct)
    {
        var exists = await _changeRequestRepo.Exists(x => x.Id == dto.Id, ct);
        if (exists == false)
        {
            return;
        }

        _changeRequestRepo.Delete(dto);
        await _unitOfWork.SaveChangesAsync(ct);
        
    }

    public async Task NotifyRequestPendingDeletion(ChangeRequestDto changeRequest, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task ClearData(CancellationToken ct)
    {        
        await _changeRequestRepo.ClearAllData(ct: ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
