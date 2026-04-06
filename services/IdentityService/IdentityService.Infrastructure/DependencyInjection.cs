using IdentityService.Application.Common.Behaviors;
using IdentityService.Application.Common.Interfaces;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Persistence.Repositories;
using IdentityService.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<IdentityDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("IdentityDb")));

        // UnitOfWork — IdentityDbContext نفسه بيعمل SaveChanges
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<IdentityDbContext>()
              as IUnitOfWork
            ?? throw new InvalidOperationException());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<IJwtService, JwtService>();

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(IdentityService.Application.Features.Auth
                       .Commands.Register.RegisterCommand).Assembly));

        // Validation Pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>),
                              typeof(ValidationBehavior<,>));

        // FluentValidation
        services.AddValidatorsFromAssembly(
            typeof(IdentityService.Application.Features.Auth
                   .Commands.Register.RegisterCommandValidator).Assembly);

        return services;
    }
}