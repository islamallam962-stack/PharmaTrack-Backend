using ExpiryTrackerService.Application.Common.Interfaces;
using ExpiryTrackerService.Application.Jobs;
using ExpiryTrackerService.Infrastructure.ExternalReaders;
using ExpiryTrackerService.Infrastructure.Persistence;
using ExpiryTrackerService.Infrastructure.Persistence.Repositories;
using ExpiryTrackerService.Infrastructure.Publishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpiryTrackerService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ExpiryTrackerDbContext>(opts =>
            opts.UseNpgsql(
                configuration.GetConnectionString("ExpiryTrackerDb")));
	services.AddHttpClient();  // زوده مع الـ registrations التانية

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ExpiryTrackerDbContext>());

        services.AddScoped<IExpiryAlertRepository, ExpiryAlertRepository>();
        services.AddScoped<IInventoryReader,        InventoryReader>();
        services.AddScoped<INotificationPublisher,  NotificationPublisher>();

        // الـ Job نفسه
        services.AddScoped<ExpiryCheckJob>();

        return services;
    }
}