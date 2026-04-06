using ExpiryTrackerService.Application.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpiryTrackerService.Worker.Workers;

public class ExpiryCheckerWorker : BackgroundService
{
    private readonly IServiceScopeFactory            _scopeFactory;
    private readonly ILogger<ExpiryCheckerWorker>    _logger;

    // بيشتغل كل 24 ساعة
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public ExpiryCheckerWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpiryCheckerWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("ExpiryCheckerWorker started.");

        // شغّل مرة فور ما الـ service يقوم
        await RunJobAsync(ct);

        // بعدين كل 24 ساعة
        using var timer = new PeriodicTimer(_interval);
        while (await timer.WaitForNextTickAsync(ct))
        {
            await RunJobAsync(ct);
        }
    }

    private async Task RunJobAsync(CancellationToken ct)
    {
        try
        {
            // كل مرة بنعمل scope جديد عشان الـ scoped services
            using var scope = _scopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService<ExpiryCheckJob>();
            await job.RunAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in ExpiryCheckJob.");
        }
    }
}