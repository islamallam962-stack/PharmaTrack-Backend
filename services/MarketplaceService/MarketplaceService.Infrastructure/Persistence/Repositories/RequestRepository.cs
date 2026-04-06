using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Domain.Entities;
using MarketplaceService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceService.Infrastructure.Persistence.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly MarketplaceDbContext _ctx;

    public RequestRepository(MarketplaceDbContext ctx) => _ctx = ctx;

    public Task<MarketplaceRequest?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Requests.FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<List<MarketplaceRequest>> GetOpenByProductAsync(
        string productName, CancellationToken ct)
        => _ctx.Requests
               .Where(r => r.Status == RequestStatus.Open &&
                           r.ProductName.ToLower() == productName.ToLower())
               .ToListAsync(ct);

    public Task<List<MarketplaceRequest>> GetByBuyerAsync(
        Guid pharmacyId, CancellationToken ct)
        => _ctx.Requests
               .Where(r => r.BuyerPharmacyId == pharmacyId)
               .OrderByDescending(r => r.CreatedAt)
               .ToListAsync(ct);

    public async Task AddAsync(MarketplaceRequest request, CancellationToken ct)
        => await _ctx.Requests.AddAsync(request, ct);

    public void Update(MarketplaceRequest request)
        => _ctx.Requests.Update(request);
}