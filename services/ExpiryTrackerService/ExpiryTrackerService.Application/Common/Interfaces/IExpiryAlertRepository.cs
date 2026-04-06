using ExpiryTrackerService.Domain.Entities;

namespace ExpiryTrackerService.Application.Common.Interfaces;

public interface IExpiryAlertRepository
{
    Task<bool> AlertExistsAsync(Guid batchId, CancellationToken ct = default);
    Task AddAsync(ExpiryAlert alert, CancellationToken ct = default);
    Task<List<ExpiryAlert>> GetPendingAsync(CancellationToken ct = default);
    void Update(ExpiryAlert alert);
}