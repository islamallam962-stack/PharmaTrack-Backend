using InventoryService.Application.Common.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence.Repositories;

public class BatchRepository : IBatchRepository
{
    private readonly InventoryDbContext _ctx;

    public BatchRepository(InventoryDbContext ctx) => _ctx = ctx;

    public Task<ProductBatch?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Batches.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<ProductBatch?> GetByQrCodeAsync(string qrCode, CancellationToken ct)
        => _ctx.Batches.FirstOrDefaultAsync(b => b.QrCode == qrCode, ct);

    public Task<List<ProductBatch>> GetNearExpiryAsync(
        int daysThreshold, CancellationToken ct)
    {
        var threshold = DateTime.UtcNow.AddDays(daysThreshold);
        return _ctx.Batches
                   .Include(b => b.Product)
                   .Where(b => b.ExpiryDate <= threshold
                            && b.ExpiryDate > DateTime.UtcNow)
                   .OrderBy(b => b.ExpiryDate)
                   .ToListAsync(ct);
    }

    public async Task AddAsync(ProductBatch batch, CancellationToken ct)
        => await _ctx.Batches.AddAsync(batch, ct);

    public void Update(ProductBatch batch)
        => _ctx.Batches.Update(batch);
}