using MarketplaceService.Domain.Entities;

namespace MarketplaceService.Application.Common.Interfaces;

public interface IRequestRepository
{
    Task<MarketplaceRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<MarketplaceRequest>> GetOpenByProductAsync(
        string productName, CancellationToken ct = default);
    Task<List<MarketplaceRequest>> GetByBuyerAsync(
        Guid pharmacyId, CancellationToken ct = default);
    Task AddAsync(MarketplaceRequest request, CancellationToken ct = default);
    void Update(MarketplaceRequest request);
}