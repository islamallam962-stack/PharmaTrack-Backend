using InventoryService.Application.Common.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryDbContext : DbContext, IUnitOfWork
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) { }

    public DbSet<Product>      Products => Set<Product>();
    public DbSet<ProductBatch> Batches  => Set<ProductBatch>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("inventory");

        builder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.ScientificName).HasMaxLength(200);
            e.Property(p => p.Manufacturer).HasMaxLength(200);
            e.Property(p => p.Category).HasMaxLength(100);
            e.HasIndex(p => p.PharmacyId);

            e.HasMany(p => p.Batches)
             .WithOne(b => b.Product)
             .HasForeignKey(b => b.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProductBatch>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.BatchNumber).IsRequired().HasMaxLength(50);
            e.Property(b => b.PurchasePrice).HasPrecision(18, 2);
            e.Property(b => b.SellingPrice).HasPrecision(18, 2);
            e.Property(b => b.Status).HasConversion<int>();
            e.HasIndex(b => b.ExpiryDate);
            e.HasIndex(b => b.QrCode);
        });
    }
}