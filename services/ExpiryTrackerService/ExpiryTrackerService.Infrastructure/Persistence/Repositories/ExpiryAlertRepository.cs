using ExpiryTrackerService.Application.Common.Interfaces;
using ExpiryTrackerService.Domain.Entities;
using ExpiryTrackerService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpiryTrackerService.Infrastructure.Persistence.Repositories;

public class ExpiryAlertRepository : IExpiryAlertRepository
{
    private readonly ExpiryTrackerDbContext _ctx;

    public ExpiryAlertRepository(ExpiryTrackerDbContext ctx) => _ctx = ctx;

    // بنتحقق إن مفيش alert اتبعتت لنفس الـ batch النهارده
    public Task<bool> AlertExistsAsync(Guid batchId, CancellationToken ct)
        => _ctx.ExpiryAlerts.AnyAsync(
            a => a.BatchId == batchId
              && a.CreatedAt.Date == DateTime.UtcNow.Date, ct);

    public async Task AddAsync(ExpiryAlert alert, CancellationToken ct)
        => await _ctx.ExpiryAlerts.AddAsync(alert, ct);

    public Task<List<ExpiryAlert>> GetPendingAsync(CancellationToken ct)
        => _ctx.ExpiryAlerts
               .Where(a => a.Status == AlertStatus.Pending)
               .ToListAsync(ct);

    public void Update(ExpiryAlert alert)
        => _ctx.ExpiryAlerts.Update(alert);
}