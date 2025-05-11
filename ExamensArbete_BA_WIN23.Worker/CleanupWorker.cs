using ExamensArbete_BA_WIN23.API.Services;

namespace ExamensArbete_BA_WIN23.Worker;

public class CleanupWorker : IHostedService
{
    public const int EXPIRATION_DELETE_DAYS = -90;
    public const int EXPIRATION_NOTIFICATION_DAYS = -60;

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
        var changeRequestService = scope.ServiceProvider.GetRequiredService<IChangeRequestService>();
        var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeed>();

        try
        {
            // ---ONLY-FOR-DEMO-Purposes----
            await dbSeeder.RemoveSeed(ct);
            await dbSeeder.SeedAsync();
            // -----------------------------

            await HandleChangeRequestsToDelete(changeRequestService, ct);
            await HandleChangeRequestsToNotify(changeRequestService, ct);
            _logger.LogInformation("{Worker} finished succesfully", nameof(CleanupWorker));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Worker} caught exception", nameof(CleanupWorker));
        }
        finally
        {
            _applicationLifetime.StopApplication();
        }
    }
    public async Task HandleChangeRequestsToDelete(IChangeRequestService changeRequestService , CancellationToken ct)
    {
        var changeRequests = await changeRequestService.GetExpiredChangeRequest(TimeSpan.FromDays(EXPIRATION_DELETE_DAYS), ct);
        if (!changeRequests.Any())
        {
            return;
        }

        await changeRequestService.DeleteExpiredChangeRequests(changeRequests, ct);
    }

    public async Task HandleChangeRequestsToNotify(IChangeRequestService changeRequestService, CancellationToken ct)
    {
        var changeRequests = await changeRequestService.GetExpiredChangeRequest(TimeSpan.FromDays(EXPIRATION_NOTIFICATION_DAYS), ct);
        if (!changeRequests.Any())
        {
            return;
        }
        
        await changeRequestService.NotifyRequestsPendingDeletion(changeRequests, ct);
    }

    public Task StopAsync(CancellationToken ct)
    {
        _logger.LogInformation("{Worker} stopped", nameof(CleanupWorker));
        return Task.CompletedTask;
    }
}
