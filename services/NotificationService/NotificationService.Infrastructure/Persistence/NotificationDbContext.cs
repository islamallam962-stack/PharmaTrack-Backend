using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Common.Interfaces;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence;

public class NotificationDbContext : DbContext, IUnitOfWork
{
    public NotificationDbContext(
        DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("notification");

        builder.Entity<NotificationLog>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Title).IsRequired().HasMaxLength(200);
            e.Property(n => n.Message).IsRequired().HasMaxLength(1000);
            e.Property(n => n.Channel).HasConversion<int>();
            e.Property(n => n.Status).HasConversion<int>();
            e.HasIndex(n => n.RecipientId);
            e.HasIndex(n => n.CreatedAt);
        });
    }
}