using InventoryService.Domain.Entities;

namespace InventoryService.Application.Common.Interfaces;

public interface IBatchRepository
{
    Task<ProductBatch?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProductBatch?> GetByQrCodeAsync(string qrCode, CancellationToken ct = default);
    Task<List<ProductBatch>> GetNearExpiryAsync(int daysThreshold, CancellationToken ct = default);
    Task AddAsync(ProductBatch batch, CancellationToken ct = default);
    void Update(ProductBatch batch);
}