using ExpiryTrackerService.Domain.Entities;

namespace ExpiryTrackerService.Application.Common.Interfaces;

public interface INotificationPublisher
{
    Task PublishExpiryAlertAsync(ExpiryAlert alert, CancellationToken ct = default);
}