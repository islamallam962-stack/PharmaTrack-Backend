using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Common.Interfaces;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence.Repositories;

public class NotificationLogRepository : INotificationLogRepository
{
    private readonly NotificationDbContext _ctx;

    public NotificationLogRepository(NotificationDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(NotificationLog log, CancellationToken ct)
        => await _ctx.NotificationLogs.AddAsync(log, ct);

    public void Update(NotificationLog log)
        => _ctx.NotificationLogs.Update(log);

    public Task<List<NotificationLog>> GetByRecipientAsync(
        Guid recipientId, CancellationToken ct)
        => _ctx.NotificationLogs
               .Where(n => n.RecipientId == recipientId)
               .OrderByDescending(n => n.CreatedAt)
               .Take(50)
               .ToListAsync(ct);
}