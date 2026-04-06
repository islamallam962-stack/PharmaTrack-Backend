namespace NotificationService.Application.Common.Interfaces;

public interface ISmsSender
{
    Task SendAsync(string toPhone, string message, CancellationToken ct = default);
}