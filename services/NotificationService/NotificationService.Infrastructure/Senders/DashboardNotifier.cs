using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Infrastructure.Senders;

public class DashboardNotifier : IDashboardNotifier
{
    private readonly IDashboardHub _hub;

    public DashboardNotifier(IDashboardHub hub) => _hub = hub;

    public Task SendAsync(
        Guid pharmacyId, string title, string message, CancellationToken ct)
        => _hub.SendToPharmacyAsync(pharmacyId, title, message, ct);
}