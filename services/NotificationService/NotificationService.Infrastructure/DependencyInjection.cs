using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Common.Behaviors;
using NotificationService.Application.Common.Interfaces;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Persistence.Repositories;
using NotificationService.Infrastructure.Senders;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NotificationDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("NotificationDb")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<NotificationDbContext>());

        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();
        services.AddScoped<IEmailSender,       EmailSender>();
        services.AddScoped<ISmsSender,         SmsSender>();
        services.AddScoped<IPushSender,        PushSender>();
        services.AddScoped<IDashboardNotifier, DashboardNotifier>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(NotificationService.Application.Features.Notifications
                       .Commands.SendExpiryAlert.SendExpiryAlertCommand).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>),
                              typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(
            typeof(NotificationService.Application.Features.Notifications
                   .Commands.SendExpiryAlert.SendExpiryAlertCommand).Assembly);

        return services;
    }
}