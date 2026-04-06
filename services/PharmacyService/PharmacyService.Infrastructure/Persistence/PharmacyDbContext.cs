using Microsoft.EntityFrameworkCore;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Domain.Entities;

namespace PharmacyService.Infrastructure.Persistence;

public class PharmacyDbContext : DbContext, IUnitOfWork
{
    public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
        : base(options) { }

    public DbSet<Pharmacy>       Pharmacies => Set<Pharmacy>();
    public DbSet<PharmacyBranch> Branches   => Set<PharmacyBranch>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("pharmacy");

        builder.Entity<Pharmacy>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.LicenseNumber).IsRequired().HasMaxLength(50);
            e.Property(p => p.Email).IsRequired().HasMaxLength(200);
            e.Property(p => p.Phone).IsRequired().HasMaxLength(20);
            e.Property(p => p.Status).HasConversion<int>();
            e.HasIndex(p => p.LicenseNumber).IsUnique();
            e.HasIndex(p => p.OwnerId);

            e.HasMany(p => p.Branches)
             .WithOne(b => b.Pharmacy)
             .HasForeignKey(b => b.PharmacyId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<PharmacyBranch>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Name).IsRequired().HasMaxLength(200);
            e.Property(b => b.Address).IsRequired().HasMaxLength(500);
            e.Property(b => b.Phone).IsRequired().HasMaxLength(20);
        });
    }
}