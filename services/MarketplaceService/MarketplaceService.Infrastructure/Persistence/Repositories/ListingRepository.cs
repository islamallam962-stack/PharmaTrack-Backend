using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Domain.Entities;
using MarketplaceService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceService.Infrastructure.Persistence.Repositories;

public class ListingRepository : IListingRepository
{
    private readonly MarketplaceDbContext _ctx;

    public ListingRepository(MarketplaceDbContext ctx) => _ctx = ctx;

    public Task<MarketplaceListing?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Listings.FirstOrDefaultAsync(l => l.Id == id, ct);

    public Task<List<MarketplaceListing>> GetAvailableAsync(
        string? productName, int page, int pageSize, CancellationToken ct)
    {
        var query = _ctx.Listings
            .Where(l => l.Status == ListingStatus.Active);

        if (!string.IsNullOrWhiteSpace(productName))
            query = query.Where(l =>
                l.ProductName.ToLower().Contains(productName.ToLower()));

        return query
            .OrderBy(l => l.ExpiryDate)   // الأقرب للانتهاء أول
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAvailableAsync(string? productName, CancellationToken ct)
    {
        var query = _ctx.Listings
            .Where(l => l.Status == ListingStatus.Active);

        if (!string.IsNullOrWhiteSpace(productName))
            query = query.Where(l =>
                l.ProductName.ToLower().Contains(productName.ToLower()));

        return query.CountAsync(ct);
    }

    public Task<List<MarketplaceListing>> GetBySellerAsync(
        Guid pharmacyId, CancellationToken ct)
        => _ctx.Listings
               .Where(l => l.SellerPharmacyId == pharmacyId)
               .OrderByDescending(l => l.CreatedAt)
               .ToListAsync(ct);

    public Task<bool> BatchAlreadyListedAsync(Guid batchId, CancellationToken ct)
        => _ctx.Listings.AnyAsync(
            l => l.BatchId == batchId &&
                 l.Status != ListingStatus.Cancelled &&
                 l.Status != ListingStatus.Sold, ct);

    public async Task AddAsync(MarketplaceListing listing, CancellationToken ct)
        => await _ctx.Listings.AddAsync(listing, ct);

    public void Update(MarketplaceListing listing)
        => _ctx.Listings.Update(listing);
}