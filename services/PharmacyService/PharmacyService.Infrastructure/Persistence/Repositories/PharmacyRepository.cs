using Microsoft.EntityFrameworkCore;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Infrastructure.Persistence.Repositories;

public class PharmacyRepository : IPharmacyRepository
{
    private readonly PharmacyDbContext _ctx;

    public PharmacyRepository(PharmacyDbContext ctx) => _ctx = ctx;

    public Task<Pharmacy?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Pharmacies
               .Include(p => p.Branches)
               .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Pharmacy?> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct)
        => _ctx.Pharmacies
               .Include(p => p.Branches)
               .FirstOrDefaultAsync(p => p.OwnerId == ownerId, ct);

    public Task<bool> ExistsByLicenseAsync(string licenseNumber, CancellationToken ct)
        => _ctx.Pharmacies
               .AnyAsync(p => p.LicenseNumber == licenseNumber.ToUpper(), ct);

    public Task<List<Pharmacy>> GetAllAsync(int page, int pageSize, CancellationToken ct)
        => _ctx.Pharmacies
               .Include(p => p.Branches)
               .OrderByDescending(p => p.CreatedAt)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(ct);

    public Task<int> CountAsync(CancellationToken ct)
        => _ctx.Pharmacies.CountAsync(ct);

    public async Task AddAsync(Pharmacy pharmacy, CancellationToken ct)
        => await _ctx.Pharmacies.AddAsync(pharmacy, ct);

    public void Update(Pharmacy pharmacy)
        => _ctx.Pharmacies.Update(pharmacy);
}