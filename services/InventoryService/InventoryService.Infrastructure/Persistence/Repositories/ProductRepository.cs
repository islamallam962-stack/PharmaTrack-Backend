using InventoryService.Application.Common.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _ctx;

    public ProductRepository(InventoryDbContext ctx) => _ctx = ctx;

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
        => _ctx.Products
               .Include(p => p.Batches)
               .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Product?> GetByNameAndPharmacyAsync(
        string name, Guid pharmacyId, CancellationToken ct)
        => _ctx.Products
               .Include(p => p.Batches)
               .FirstOrDefaultAsync(
                   p => p.Name == name && p.PharmacyId == pharmacyId, ct);

    public Task<List<Product>> GetByPharmacyAsync(
        Guid pharmacyId, int page, int pageSize, CancellationToken ct)
        => _ctx.Products
               .Include(p => p.Batches)
               .Where(p => p.PharmacyId == pharmacyId)
               .OrderByDescending(p => p.CreatedAt)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(ct);

    public Task<int> CountByPharmacyAsync(Guid pharmacyId, CancellationToken ct)
        => _ctx.Products.CountAsync(p => p.PharmacyId == pharmacyId, ct);

    public async Task AddAsync(Product product, CancellationToken ct)
        => await _ctx.Products.AddAsync(product, ct);

    public void Update(Product product)
        => _ctx.Products.Update(product);

    public void Delete(Product product)
        => _ctx.Products.Remove(product);
}