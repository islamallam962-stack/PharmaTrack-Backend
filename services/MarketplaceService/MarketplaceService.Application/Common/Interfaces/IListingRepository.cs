using MarketplaceService.Domain.Entities;
using MarketplaceService.Domain.Enums;

namespace MarketplaceService.Application.Common.Interfaces;

public interface IListingRepository
{
    Task<MarketplaceListing?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<MarketplaceListing>> GetAvailableAsync(
        string? productName, int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAvailableAsync(string? productName, CancellationToken ct = default);
    Task<List<MarketplaceListing>> GetBySellerAsync(
        Guid pharmacyId, CancellationToken ct = default);
    Task<bool> BatchAlreadyListedAsync(Guid batchId, CancellationToken ct = default);
    Task AddAsync(MarketplaceListing listing, CancellationToken ct = default);
    void Update(MarketplaceListing listing);
}