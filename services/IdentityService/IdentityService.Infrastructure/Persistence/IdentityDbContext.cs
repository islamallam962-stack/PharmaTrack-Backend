using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using IdentityService.Application.Common.Interfaces;
namespace IdentityService.Infrastructure.Persistence;

public class IdentityDbContext : DbContext, IUnitOfWork
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);

            e.Property(u => u.FullName)
             .IsRequired()
             .HasMaxLength(100);

            e.Property(u => u.Email)
             .IsRequired()
             .HasMaxLength(200);

            e.HasIndex(u => u.Email)
             .IsUnique();

            e.Property(u => u.Role)
             .HasConversion<int>();
        });
    }
}