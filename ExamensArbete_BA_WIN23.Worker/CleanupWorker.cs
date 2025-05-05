using ExamensArbete_BA_WIN23.Services;

namespace ExamensArbete_BA_WIN23.Worker;

public class CleanupWorker : IHostedService
{
    public const int EXPIRATION_NOTIFICATION_DAYS = -60;
    public const int EXPIRATION_DELETE_DAYS = -90;

    private readonly ILogger<CleanupWorker> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _scopeFactory;

    public CleanupWorker(
        ILogger<CleanupWorker> logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _logger.LogInformation("{Worker} started", nameof(CleanupWorker));
        var scope = _scopeFactory.CreateScope();
        var changeRequestService = scope.ServiceProvider.GetRequiredService<ChangeRequestService>();

        try
        {
            await HandleChangeRequestsToDelete(changeRequestService, ct);
            await HandleChangeRequestsToNotify(changeRequestService, ct);
            _logger.LogInformation("{Worker} finished succesfully", nameof(CleanupWorker));
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "{Worker} caught exception", nameof(CleanupWorker));
        }
        finally
        {
            await ClearData(changeRequestService, ct);
            _applicationLifetime.StopApplication();
        }
    }

    private async Task ClearData(ChangeRequestService changeRequestService, CancellationToken ct)
    {       
        await changeRequestService.ClearData(ct);
    }

    public async Task HandleChangeRequestsToDelete(ChangeRequestService changeRequestService , CancellationToken ct)
    {
        var changeRequests = await changeRequestService.GetExpiredChangeRequest(TimeSpan.FromDays(EXPIRATION_DELETE_DAYS), ct);
        if (!changeRequests.Any())
        {
            return;
        }


        foreach (var changeRequest in changeRequests)
        {
            await changeRequestService.DeleteChangeRequest(changeRequest, ct);
            _logger.LogInformation("{changeRequest} was deleted", changeRequest.Id);
        }

        _logger.LogInformation("Deleted {Count} changeRequests", changeRequests.Count());
    }

    public async Task HandleChangeRequestsToNotify(ChangeRequestService changeRequestService, CancellationToken ct)
    {
        var changeRequests = await changeRequestService.GetExpiredChangeRequest(TimeSpan.FromDays(EXPIRATION_NOTIFICATION_DAYS), ct);
        if (!changeRequests.Any())
        {
            return;
        }

        foreach (var changeRequest in changeRequests)
        {
            await changeRequestService.NotifyRequestPendingDeletion(changeRequest, ct);
            _logger.LogInformation("Board members of {changeRequest} were sent notifications", changeRequest.Id);
        }
    }

    public Task StopAsync(CancellationToken ct)
    {
        _logger.LogInformation("{Worker} stopped", nameof(CleanupWorker));
        return Task.CompletedTask;
    }
}
