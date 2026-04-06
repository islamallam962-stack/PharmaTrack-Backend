namespace NotificationService.Application.Common.Interfaces;

public interface IDashboardNotifier
{
    Task SendAsync(Guid pharmacyId, string title, string message, CancellationToken ct = default);
}