using ExpiryTrackerService.Application.Common.Interfaces;
using ExpiryTrackerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpiryTrackerService.Infrastructure.Persistence;

public class ExpiryTrackerDbContext : DbContext, IUnitOfWork
{
    public ExpiryTrackerDbContext(
        DbContextOptions<ExpiryTrackerDbContext> options)
        : base(options) { }

    public DbSet<ExpiryAlert> ExpiryAlerts => Set<ExpiryAlert>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("expiry");

        builder.Entity<ExpiryAlert>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ProductName).IsRequired().HasMaxLength(200);
            e.Property(a => a.BatchNumber).IsRequired().HasMaxLength(50);
            e.Property(a => a.Status).HasConversion<int>();

            // index عشان نتحقق بسرعة إن الـ alert متبعتتش قبل كده
            e.HasIndex(a => new { a.BatchId, a.CreatedAt });
            e.HasIndex(a => a.PharmacyId);
        });
    }
}