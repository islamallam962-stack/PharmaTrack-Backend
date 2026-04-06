namespace NotificationService.Application.Common.Interfaces;

public interface IDashboardHub
{
    Task SendToPharmacyAsync(
        Guid pharmacyId, string title, string message, CancellationToken ct);
}