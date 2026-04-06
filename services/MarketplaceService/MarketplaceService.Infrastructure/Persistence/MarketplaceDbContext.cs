using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceService.Infrastructure.Persistence;

public class MarketplaceDbContext : DbContext, IUnitOfWork
{
    public MarketplaceDbContext(
        DbContextOptions<MarketplaceDbContext> options) : base(options) { }

    public DbSet<MarketplaceListing> Listings => Set<MarketplaceListing>();
    public DbSet<MarketplaceRequest> Requests => Set<MarketplaceRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("marketplace");

        builder.Entity<MarketplaceListing>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.ProductName).IsRequired().HasMaxLength(200);
            e.Property(l => l.BatchNumber).IsRequired().HasMaxLength(50);
            e.Property(l => l.OriginalPrice).HasPrecision(18, 2);
            e.Property(l => l.DiscountedPrice).HasPrecision(18, 2);
            e.Property(l => l.Status).HasConversion<int>();
            e.HasIndex(l => l.Status);
            e.HasIndex(l => l.ProductName);
            e.HasIndex(l => l.BatchId).IsUnique();
            e.HasIndex(l => l.ExpiryDate);
        });

        builder.Entity<MarketplaceRequest>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.ProductName).IsRequired().HasMaxLength(200);
            e.Property(r => r.MaxPrice).HasPrecision(18, 2);
            e.Property(r => r.Status).HasConversion<int>();
            e.HasIndex(r => r.Status);
            e.HasIndex(r => r.ProductName);
            e.HasIndex(r => r.BuyerPharmacyId);
        });
    }
}