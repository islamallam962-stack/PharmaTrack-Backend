using ExpiryTrackerService.Application.Common.Interfaces;
using ExpiryTrackerService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ExpiryTrackerService.Application.Jobs;

public class ExpiryCheckJob
{
    private readonly IInventoryReader        _inventory;
    private readonly IExpiryAlertRepository  _alerts;
    private readonly INotificationPublisher  _publisher;
    private readonly IUnitOfWork             _uow;
    private readonly ILogger<ExpiryCheckJob> _logger;

    private const int DaysThreshold = 90;

    public ExpiryCheckJob(
        IInventoryReader inventory,
        IExpiryAlertRepository alerts,
        INotificationPublisher publisher,
        IUnitOfWork uow,
        ILogger<ExpiryCheckJob> logger)
    {
        _inventory = inventory;
        _alerts    = alerts;
        _publisher = publisher;
        _uow       = uow;
        _logger    = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation(
            "Expiry check job started at {Time}", DateTime.UtcNow);

        var batches = await _inventory
            .GetNearExpiryBatchesAsync(DaysThreshold, ct);

        _logger.LogInformation(
            "Found {Count} batches near expiry.", batches.Count);

        foreach (var batch in batches)
        {
            // متبعتش alert لنفس الـ batch النهارده
            var exists = await _alerts.AlertExistsAsync(batch.BatchId, ct);
            if (exists) continue;

            var alert = ExpiryAlert.Create(
                batch.PharmacyId,
                batch.BatchId,
                batch.ProductName,
                batch.BatchNumber,
                batch.ExpiryDate);

            await _alerts.AddAsync(alert, ct);
            await _uow.SaveChangesAsync(ct);

            // نشر الـ notification
            await _publisher.PublishExpiryAlertAsync(alert, ct);

            alert.MarkAsSent();
            _alerts.Update(alert);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Alert sent for batch {BatchNumber} — {DaysToExpiry} days left.",
                alert.BatchNumber, alert.DaysToExpiry);
        }

        _logger.LogInformation(
            "Expiry check job finished at {Time}", DateTime.UtcNow);
    }
}