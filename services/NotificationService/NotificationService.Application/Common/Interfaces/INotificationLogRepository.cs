using NotificationService.Domain.Entities;

namespace NotificationService.Application.Common.Interfaces;

public interface INotificationLogRepository
{
    Task AddAsync(NotificationLog log, CancellationToken ct = default);
    void Update(NotificationLog log);
    Task<List<NotificationLog>> GetByRecipientAsync(
        Guid recipientId, CancellationToken ct = default);
}