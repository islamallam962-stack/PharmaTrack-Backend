using ExpiryTrackerService.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpiryTrackerService.Infrastructure.ExternalReaders;

public class InventoryReader : IInventoryReader
{
    private readonly IConfiguration _config;

    public InventoryReader(IConfiguration config) => _config = config;

    public async Task<List<NearExpiryBatchDto>> GetNearExpiryBatchesAsync(
        int daysThreshold,
        CancellationToken ct)
    {
        var threshold = DateTime.UtcNow.AddDays(daysThreshold);
        var connStr   = _config.GetConnectionString("InventoryDb")!;

        var options = new DbContextOptionsBuilder<InventoryReaderContext>()
            .UseNpgsql(connStr)
            .Options;

        await using var ctx = new InventoryReaderContext(options);

        return await ctx.Batches
            .Where(b => b.ExpiryDate <= threshold
                     && b.ExpiryDate > DateTime.UtcNow
                     && b.Quantity > 0)
            .Select(b => new NearExpiryBatchDto(
                b.Id,
                b.ProductId,
                b.Product.PharmacyId,
                b.Product.Name,
                b.BatchNumber,
                b.ExpiryDate,
                b.Quantity))
            .ToListAsync(ct);
    }
}

// Read-only context بيقرأ من الـ inventory schema
public class InventoryReaderContext : DbContext
{
    public InventoryReaderContext(DbContextOptions<InventoryReaderContext> opts)
        : base(opts) { }

    public DbSet<BatchReadModel>   Batches  => Set<BatchReadModel>();
    public DbSet<ProductReadModel> Products => Set<ProductReadModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("inventory");

        builder.Entity<ProductReadModel>(e =>
        {
            e.ToTable("Products");
            e.HasKey(p => p.Id);
            e.HasMany(p => p.Batches)
             .WithOne(b => b.Product)
             .HasForeignKey(b => b.ProductId);
        });

        builder.Entity<BatchReadModel>(e =>
        {
            e.ToTable("Batches");
            e.HasKey(b => b.Id);
        });
    }
}

public class ProductReadModel
{
    public Guid   Id         { get; set; }
    public string Name       { get; set; } = default!;
    public Guid   PharmacyId { get; set; }
    public List<BatchReadModel> Batches { get; set; } = new();
}

public class BatchReadModel
{
    public Guid     Id         { get; set; }
    public Guid     ProductId  { get; set; }
    public string   BatchNumber { get; set; } = default!;
    public int      Quantity   { get; set; }
    public DateTime ExpiryDate { get; set; }
    public ProductReadModel Product { get; set; } = default!;
}