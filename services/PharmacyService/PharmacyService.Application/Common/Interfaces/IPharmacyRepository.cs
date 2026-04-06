using PharmacyService.Domain.Entities;

namespace PharmacyService.Application.Common.Interfaces;

public interface IPharmacyRepository
{
    Task<Pharmacy?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Pharmacy?> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
    Task<bool> ExistsByLicenseAsync(string licenseNumber, CancellationToken ct = default);
    Task<List<Pharmacy>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task AddAsync(Pharmacy pharmacy, CancellationToken ct = default);
    void Update(Pharmacy pharmacy);
}