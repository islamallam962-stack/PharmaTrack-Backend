using InventoryService.Domain.Entities;

namespace InventoryService.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Product?> GetByNameAndPharmacyAsync(string name, Guid pharmacyId, CancellationToken ct = default);
    Task<List<Product>> GetByPharmacyAsync(Guid pharmacyId, int page, int pageSize, CancellationToken ct = default);
    Task<int> CountByPharmacyAsync(Guid pharmacyId, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    void Update(Product product);
    void Delete(Product product);
}